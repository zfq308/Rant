using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rant.Internal.VM.Compiler.Parselets
{
    internal class PatternParselet : IParselet
    {
        public IEnumerator<IParselet> Parse(TokenReader reader, BytecodeGenerator generator)
        {
            while(!reader.End)
            {
                var token = reader.PeekLooseToken();
                switch(token.ID)
                {
                    case R.EscapeSequence:
                        yield return new EscapeParselet();
                        break;
                    default:
                        throw new RantCompilerException(reader.SourceName, token, "Unexpected token: " + token.ID);
                }
            }
            yield break;
        }
    }
}
