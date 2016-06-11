using System.Collections.Generic;

using Rant.Internal.VM.Instructions;

namespace Rant.Internal.VM.Compiler.Parselets
{
    internal class PatternParselet : IParselet
    {
        public IEnumerator<IParselet> Parse(TokenReader reader, BytecodeGenerator generator)
        {
            while(!reader.End)
            {
                var token = reader.PeekToken();
                switch(token.ID)
                {
                    // escape sequences
                    case R.EscapeSequence:
                        yield return new EscapeParselet();
                        break;
                    // non-special text
                    default:
                        reader.ReadToken();
                        generator.LatestSegment.AddStringReference(RantOpCode.PrintStringConstant, token.Value);
                        break;
                }
            }
            yield break;
        }
    }
}
