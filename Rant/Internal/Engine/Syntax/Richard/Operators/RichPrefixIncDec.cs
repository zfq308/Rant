﻿using System.Collections.Generic;

using Rant.Internal.Engine.ObjectModel;
using Rant.Internal.Stringes;

namespace Rant.Internal.Engine.Syntax.Richard.Operators
{
    internal class RichPrefixIncDec : RichActionBase
    {
        public RichActionBase RightSide;
        public bool Increment = true;

        public RichPrefixIncDec(Stringe token)
            : base(token)
        {

        }

        public override object GetValue(Sandbox sb)
        {
            if (!(RightSide is RichVariable))
                throw new RantRuntimeException(sb.Pattern, Range,
                    "Right side of prefix " + (Increment ? "increment" : "decrement") + " operator must be a variable.");
            string name = (RightSide as RichVariable).Name;
            double newValue = -1;
            if (sb.Objects[name] != null && sb.Objects[name].Type == RantObjectType.Number)
            {
                newValue = ((double)sb.Objects[name].Value);
                newValue = (Increment ? newValue + 1 : newValue - 1);
                sb.Objects[name] = new RantObject(newValue);
            }
            else if (sb.Objects[name] == null)
                throw new RantRuntimeException(sb.Pattern, Range, "Cannot increment undefined value.");
            else
                throw new RantRuntimeException(sb.Pattern, Range, "Cannot increment value of type " + sb.Objects[name].Type + ".");
            return newValue;
        }

        public override IEnumerator<RantAction> Run(Sandbox sb)
        {
            yield break;
        }
    }
}
