using System.Collections.Generic;

using Rant.Internal.Engine.ObjectModel;
using Rant.Internal.Stringes;

namespace Rant.Internal.Engine.Syntax
{
	internal class RADefineSubroutine : RASubroutine
	{
		public Dictionary<string, SubroutineParameterType> Parameters;

		public RADefineSubroutine(Stringe name)
			: base(name)
		{ }

		public override IEnumerator<RantAction> Run(Sandbox sb)
		{
			sb.Objects[Name] = new RantObject(this);
			yield break;
		}
	}

	internal enum SubroutineParameterType
	{
		Loose,
		Greedy
	}
}
