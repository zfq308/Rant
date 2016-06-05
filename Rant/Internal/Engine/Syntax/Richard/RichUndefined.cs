using System.Collections.Generic;

using Rant.Internal.Engine.ObjectModel;
using Rant.Internal.Stringes;

namespace Rant.Internal.Engine.Syntax.Richard
{
    internal class RichUndefined : RichActionBase
    {
        public RichUndefined(Stringe token)
            : base(token)
        {

        }

        public override object GetValue(Sandbox sb)
        {
            return new RantObject(RantObjectType.Undefined);
        }

        public override IEnumerator<RantAction> Run(Sandbox sb)
        {
            yield break;
        }
    }
}
