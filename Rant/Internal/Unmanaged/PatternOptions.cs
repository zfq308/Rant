#if !UNITY

using System.Runtime.InteropServices;

namespace Rant.Internal.Unmanaged
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct PatternOptions
    {
        public double Timeout;
        public int CharLimit;
    }
}

#endif