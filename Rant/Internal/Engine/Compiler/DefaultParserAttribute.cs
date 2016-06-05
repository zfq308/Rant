using System;

namespace Rant.Internal.Engine.Compiler
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal sealed class DefaultParserAttribute : Attribute
    {
        public DefaultParserAttribute()
        {

        }
    }
}
