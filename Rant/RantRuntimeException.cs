using System;
using System.Collections.Generic;
using System.Linq;

using Rant.Internal.Engine.Compiler;
using Rant.Internal.Stringes;

namespace Rant
{
    /// <summary>
    /// Represents a runtime error raised by the Rant engine.
    /// </summary>
    public sealed class RantRuntimeException : Exception
    {
        private readonly int _line;
        private readonly int _col;
        private readonly int _index;
        private readonly RantProgram _source;

        /// <summary>
        /// The line on which the error occurred.
        /// </summary>
        public int Line => _line;

        /// <summary>
        /// The column on which the error occurred.
        /// </summary>
        public int Column => _col;

        /// <summary>
        /// The character index on which the error occurred.
        /// </summary>
        public int Index => _index;

        /// <summary>
        /// The source of the error.
        /// </summary>
        public RantProgram Source => _source;

        internal RantRuntimeException(RantProgram source, int line, int col, int index, string message = "A generic runtime error was encountered.") 
            : base(line > 0 ? (($"({source.Name} @ Ln {line}, Col {col}): ") + message) : message)
        {
            _source = source;
	        _line = line;
	        _col = col;
	        _index = index;
        }
    }
}