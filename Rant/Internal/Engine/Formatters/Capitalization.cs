﻿using Rant.Internal.Engine.Metadata;

namespace Rant.Internal.Engine.Formatters
{
    internal enum Capitalization
    {
        [RantDescription("No capitalization.")]
        None,
        [RantDescription("Convert to lowercase.")]
        Lower,
        [RantDescription("Convert to uppercase.")]
        Upper,
        [RantDescription("Convert to title case.")]
        Title,
        [RantDescription("Capitalize the first letter.")]
        First,
        [RantDescription("Capitalize the first letter of every sentence.")]
        Sentence,
        [RantDescription("Capitalize the first letter of every word.")]
        Word
    }
}
