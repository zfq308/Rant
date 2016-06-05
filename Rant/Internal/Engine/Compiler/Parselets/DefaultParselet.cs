using System.Collections.Generic;

using Rant.Internal.Engine.Syntax;
using Rant.Internal.Stringes;

namespace Rant.Internal.Engine.Compiler.Parselets
{
    [DefaultParselet]
    internal class DefaultParselet : Parselet
    {
        [DefaultParser]
        private IEnumerable<Parselet> Default(Token<R> token)
        {
            AddToOutput(new RAText(token));
            yield break;
        }
    }
}
