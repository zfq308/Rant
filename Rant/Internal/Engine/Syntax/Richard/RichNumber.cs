﻿using System.Collections.Generic;

using Rant.Internal.Stringes;

namespace Rant.Internal.Engine.Syntax.Richard
{
	internal class RichNumber : RichActionBase
	{
		public double Value;

		public RichNumber(double _value, Stringe _origin)
			: base(_origin)
		{
			Value = _value;
			Type = ActionValueType.Number;
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
            return Value.ToString();
        }
    }
}
