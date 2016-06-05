using System.Collections.Generic;

using Rant.Internal.Engine.ObjectModel;
using Rant.Internal.Stringes;

namespace Rant.Internal.Engine.Syntax.Richard
{
    internal class RichNull : RichActionBase
    {
        public RichNull(Stringe token)
            : base(token)
        {
        }

        public override object GetValue(Sandbox sb)
        {
            return new RantObject();
        }

        public override IEnumerator<RantAction> Run(Sandbox sb)
        {
            yield break;
        }
    }
}
