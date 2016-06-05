﻿using System.Collections.Generic;

using Rant.Internal.Engine.ObjectModel;
using Rant.Internal.Stringes;

namespace Rant.Internal.Engine.Syntax.Richard
{
    internal class RichNativeFunction : RichActionBase
    {
        private int _argCount;
        public RantObject That;
        private RantFunctionInfo _function;

        public int ArgCount => _argCount;
        public RantFunctionInfo Function => _function;

        public RichNativeFunction(Stringe token, int argCount, RantFunctionInfo info)
            : base(token)
        {
            _argCount = argCount;
            _function = info;
        }

        public override object GetValue(Sandbox sb)
        {
            return this;
        }

        public IEnumerator<RichActionBase> Execute(Sandbox sb)
        {
            List<object> args = new List<object>();
            for (var i = 0; i < _argCount; i++)
                args.Add(new RantObject(sb.ScriptObjectStack.Pop()));
            args.Add(That);
            args.Reverse();
            IEnumerator<RantAction> iterator = null;
            while (true)
            {
                try
                {
                    if(iterator == null)
                        iterator = _function.Invoke(sb, args.ToArray());
                    if (!iterator.MoveNext())
                        break;
                }
                // attach token to it and throw it up
                catch (RantRuntimeException e)
                {
                    e.SetToken(Range);
                    throw e;
                }
                yield return iterator.Current as RichActionBase;
            }
        }

        public override IEnumerator<RantAction> Run(Sandbox sb)
        {
            yield break;
        }
    }
}
