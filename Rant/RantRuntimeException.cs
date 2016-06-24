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
	    private readonly int _address;
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
		/// The execution address at which the error occurred.
		/// </summary>
	    public int Address => _address;

        /// <summary>
        /// The source of the error.
        /// </summary>
        public RantProgram Source => _source;

        // for compatibility, until we finish v3.
        private RantPattern _sourcePattern;

        internal RantRuntimeException(RantProgram source, int line, int col, int index, int address, string message = "A generic runtime error was encountered.") 
            : base(line > 0 ? (($"({source.Name} @ Ln {line}, Col {col}): ") + message) : message)
        {
            _source = source;
	        _line = line;
	        _col = col;
	        _index = index;
	        _address = address;
        }

        internal RantRuntimeException(RantPattern source, int line, int col, int index, string message = "A generic runtime error was encountered.")
            : base(line > 0 ? (($"({source.Name} @ Ln {line}, Col {col}): ") + message) : message)
        {
            _source = null;
            _sourcePattern = source;
            _line = line;
            _col = col;
            _index = index;
        }

        internal RantRuntimeException(RantPattern source, Stringe stringe, string message = "A generic runtime error was encountered.")
            : base(stringe.Line > 0 ? (($"({source.Name} @ Ln {stringe.Line}, Col {stringe.Column}): ") + message) : message)
        {
            _source = null;
            _sourcePattern = source;
            _line = stringe.Line;
            _col = stringe.Column;
            _index = 0;
        }
        internal RantRuntimeException(string source, Stringe stringe, string message = "A generic runtime error was encountered.")
            : base(stringe.Line > 0 ? (($"(unknown @ Ln {stringe.Line}, Col {stringe.Column}): ") + message) : message)
        {
            _source = null;
            _line = stringe.Line;
            _col = stringe.Column;
            _index = 0;
        }
    }
}