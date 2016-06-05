﻿using System.Collections.Generic;

using Rant.Internal.Stringes;

namespace Rant.Internal.Engine.Syntax.Richard
{
	internal class RichBoolean : RichActionBase
	{
		public readonly bool Value;

		public RichBoolean(Stringe token, bool value)
			: base(token)
		{
			Value = value;
			Type = ActionValueType.Boolean;
		}

		public override object GetValue(Sandbox sb)
		{
			return Value;
		}

		public override IEnumerator<RantAction> Run(Sandbox sb)
		{
			yield break;
		}

		public override string ToString()
		{
			return Value ? "yes" : "no";
		}
	}
}
