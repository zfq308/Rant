using Rant.Internal.Stringes;

namespace Rant.Internal.Engine.Syntax.Richard.Operators
{
    internal class RichModuloOperator : RichInfixOperator
    {
        public RichModuloOperator(Stringe _origin)
			: base(_origin)
		{
            Operation = (x, y) => x % y;
            Precedence = 5;
        }
    }
}
