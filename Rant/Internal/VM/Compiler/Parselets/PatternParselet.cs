using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rant.Internal.VM.Instructions;

namespace Rant.Internal.VM.Compiler.Parselets
{
    /// <summary>
    /// The main parselet.
    /// </summary>
    internal class PatternParselet : IParselet
    {
        private bool _isFunctionArgument;

        public PatternParselet(bool isFunctionArgument = false)
        {
            _isFunctionArgument = isFunctionArgument;
        }

        public IEnumerator<IParselet> Parse(TokenReader reader, BytecodeGenerator generator)
        {
            while(!reader.End)
            {
                var token = reader.PeekToken();

                if(_isFunctionArgument && (token.ID == R.Semicolon || token.ID == R.RightSquare))
                {
                    reader.ReadToken();
                    yield break;
                }

                switch(token.ID)
                {
                    // escape sequences
                    case R.EscapeSequence:
                        yield return new EscapeParselet();
                        break;
                    // tags (functions, subroutines, replacers, scripts)
                    case R.LeftSquare:
                        yield return new TagParselet();
                        break;
                    // non-special text
                    default:
                        reader.ReadToken();
                        generator.LatestSegment.AddStringReference(RantOpCode.PrintStringConstant, token.Value);
                        break;
                }
            }

            if(_isFunctionArgument)
                throw new RantCompilerException(reader.SourceName, reader.PrevToken, "Unexpected end-of-pattern.");
            yield break;
        }
    }
}
