﻿using Rant.Internal.Engine.Metadata;

namespace Rant.Internal.Engine.Formatters
{
    internal enum Endianness
    {
        [RantDescription("Big endian.")]
        Big,
        [RantDescription("Little endian.")]
        Little,
        [RantDescription("Whatever endianness your system uses.")]
        Default
    }
}