using System;
using System.Collections.Generic;

using Rant.Internal.Stringes;
using Rant.Internal.VM.Instructions;

namespace Rant.Internal.VM.Compiler.Parselets
{
    internal class FunctionParselet : IParselet
    {
        public IEnumerator<IParselet> Parse(TokenReader reader, BytecodeGenerator generator)
        {
            var name = reader.Read(R.Text, "Function name").Value;
            int argumentCount = 0;
            if(reader.PeekToken().ID == R.Colon)
            {
                reader.ReadLooseToken();
                if(reader.PeekLooseToken().ID == R.RightSquare)
                {
                    throw new RantCompilerException(reader.SourceName, reader.PeekLooseToken(), "Expected arguments, got end-of-function.");
                }

                // read arguments
                while(!reader.End)
                {
                    generator.LatestSegment.AddGeneric(RantOpCode.NewOutput);
                    yield return new PatternParselet(true);
                    generator.LatestSegment.AddGeneric(RantOpCode.ReturnOutput);
                    argumentCount++;

                    if(reader.PeekLooseToken().ID == R.RightSquare)
                    {
                        break;
                    }
                    reader.ReadLooseToken();
                }

                if(reader.End)
                {
                    throw new RantCompilerException(reader.SourceName, reader.PrevToken, "Unexpected end-of-pattern.");
                }
            }

            switch(name)
            {
                // optimize some special cases
                case "close":
                    CheckArgumentCount(reader, name, 0, argumentCount);
                    generator.LatestSegment.AddGeneric(RantOpCode.CloseChannelConstant);
                    break;
                default:
                    // TODO: add real native functions.
                    generator.LatestSegment.AddGeneric(RantOpCode.NativeCall, BitConverter.GetBytes((short)0));
                    break;
            }
            reader.Read(R.RightSquare, "End of function");
            yield break;
        }

        private void CheckArgumentCount(TokenReader reader, Stringe name, int expected, int actual)
        {
            if(expected != actual)
                throw new RantCompilerException(
                    reader.SourceName,
                    name,
                    "Error: invalid number of arguments for " + name.Value + ". Got " + actual + ", expected " + expected + ".");
        }
    }
}
