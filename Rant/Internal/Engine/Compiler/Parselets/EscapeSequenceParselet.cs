using System.Collections.Generic;

using Rant.Internal.Engine.Syntax;
using Rant.Internal.Stringes;

namespace Rant.Internal.Engine.Compiler.Parselets
{
    internal class EscapeSequenceParselet : Parselet
    {
        [TokenParser(R.EscapeSequence)]
        private IEnumerable<Parselet> EscapeSequence(Token<R> token)
        {
            AddToOutput(new RAEscape(token));
            yield break;
        }
    }
}
