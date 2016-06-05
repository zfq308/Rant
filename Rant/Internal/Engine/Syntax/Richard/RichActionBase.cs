﻿using Rant.Internal.Stringes;

namespace Rant.Internal.Engine.Syntax.Richard
{
	internal abstract class RichActionBase : RantAction
	{
		public RichActionBase(Stringe _origin)
			: base(_origin)
		{
		}

		public ActionValueType Type = ActionValueType.Null;

		public abstract object GetValue(Sandbox sb);

		internal enum ActionValueType
		{
			Null,
			Boolean,
			Number,
			String,
			List,
			Object,
			Pattern,
			Function
		};

        public bool Breakable = false;
        public bool Returnable = false;
	}
}
