using System.Collections.Generic;

using Rant.Internal.Engine.Syntax;
using Rant.Internal.Stringes;

namespace Rant.Internal.Engine.Compiler.Parselets
{
    internal class ConstantLiteralParselet : Parselet
    {
        [TokenParser(R.ConstantLiteral)]
        private IEnumerable<Parselet> ConstantLiteral(Token<R> token)
        {
            AddToOutput(new RAText(token, Util.UnescapeConstantLiteral(token.Value)));
            yield break;
        }
    }
}
