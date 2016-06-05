using Rant.Internal.Stringes;

namespace Rant.Internal.Engine.Syntax.Richard.Operators
{
	internal class RichMultiplicationOperator : RichInfixOperator
	{
		public RichMultiplicationOperator(Stringe _origin)
			: base(_origin)
		{
			Operation = (x, y) => x * y;
            Precedence = 5;
        }
	}
}
