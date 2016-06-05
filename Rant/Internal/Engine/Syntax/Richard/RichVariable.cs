﻿using System.Collections.Generic;

using Rant.Internal.Engine.ObjectModel;
using Rant.Internal.Stringes;

namespace Rant.Internal.Engine.Syntax.Richard
{
	internal class RichVariable : RichActionBase
	{
		public string Name;

		public RichVariable(Stringe _origin)
			: base(_origin)
		{
			Name = _origin.Value;
		}
		
		public override object GetValue(Sandbox sb)
		{
            if (RichardFunctions.HasObject(Name))
                return RichardFunctions.GetObject(Name);
			if (sb.Objects[Name] == null) return new RantObject(RantObjectType.Undefined);
			var obj = sb.Objects[Name];
            if (obj.Type == RantObjectType.No)
                return obj;
			return obj.Value;
		}

		public override IEnumerator<RantAction> Run(Sandbox sb)
		{
			yield break;
		}
	}
}
