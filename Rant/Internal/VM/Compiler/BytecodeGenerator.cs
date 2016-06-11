using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rant.Internal.VM.Instructions;
using Rant.Internal.Stringes;

namespace Rant.Internal.VM.Compiler
{
    /// <summary>
    /// Accepts bytecode segments in a format which allows for the program to be built as it is being parsed,
    /// and then resolves references between them and outputs the resulting bytecode.
    /// </summary>
    internal class BytecodeGenerator
    {
        /* Why do we use a HashSet *and* list? Well, it's very simple.
         * A hash set doesn't maintain a specific element order, so we can't perform
         * operations like IndexOf in any meaningful way. But, we still want to check
         * for duplicates, and HashSet is the fastest way to do that. So we combine
         * their powers!
         */
        private List<string> _stringTable;
        private HashSet<string> _stringTableSet;
        private Dictionary<int, int> _stringTableCounts;

        private List<BytecodeSegment> _program;
        private bool _debug;

        /// <summary>
        /// The latest BytecodeSegment to be added.
        /// </summary>
        public BytecodeSegment LatestSegment
        {
            get { return _program.Last(); }
        }

        /// <summary>
        /// Whether or not this will generate a debug build.
        /// </summary>
        public bool Debug => _debug;

        /// <summary>
        /// The string table.
        /// </summary>
        public List<string> StringTable => _stringTable;

        public BytecodeGenerator(bool debug)
        {
            _debug = debug;
            _stringTable = new List<string>();
            _stringTableSet = new HashSet<string>();
            _stringTableCounts = new Dictionary<int, int>();
            _program = new List<BytecodeSegment> { new BytecodeSegment(this) };
        }

        /// <summary>
        /// Compiles the pattern.
        /// </summary>
        /// <returns>The bytecode of the pattern.</returns>
        public byte[] Compile()
        {
            return _program.Last().Bytecode.ToArray();
        }

        /// <summary>
        /// Adds a string to the string table.
        /// </summary>
        /// <param name="value">The string to add.</param>
        /// <returns>The position of this string in the string table.</returns>
        public int AddString(string value)
        {
            if(_stringTableSet.Contains(value))
                return _stringTable.IndexOf(value);
            _stringTableSet.Add(value);
            _stringTable.Add(value);
            var index = _stringTable.Count - 1;
            if(_stringTableCounts.ContainsKey(index))
                _stringTableCounts[index]++;
            else
                _stringTableCounts[index] = 1;
            return index;
        }

        /// <summary>
        /// Add a bytecode segment to this generator.
        /// </summary>
        /// <param name="segment">The segment to add.</param>
        public void AddSegment(BytecodeSegment segment)
        {
            _program.Add(segment);
        }

        /// <summary>
        /// Outputs the string table as a byte array.
        /// </summary>
        /// <returns>The string table in byte array form.</returns>
        public byte[] BuildStringTable()
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            writer.Write(_stringTable.Count);
            foreach(var str in _stringTable)
            {
                writer.Write(str.Length);
                writer.Write(Encoding.UTF8.GetBytes(str));
            }
            return stream.ToArray();
        }

        /// <summary>
        /// Replaces the supplied index in the string table with the given value, provided that it is only used by one opcode.
        /// If it's used by multiple opcodes, add it as a new entry.
        /// </summary>
        /// <param name="index">The index to check.</param>
        /// <param name="value">The value to replace or add.</param>
        /// <returns>The index of the value.</returns>
        public int ReplaceIfUnused(int index, string value)
        {
            if(_stringTableCounts[index] > 1)
            {
                return AddString(value);
            }
            _stringTableSet.Remove(_stringTable[index]);
            _stringTable[index] = value;
            _stringTableSet.Add(value);
            return index;
        }

        public string GetString(int index)
        {
            return _stringTable[index];
        }
    }

    internal class BytecodeSegment
    {
        private List<byte> _bytecode = new List<byte>();
        private Dictionary<int, int> _stringReferences = new Dictionary<int, int>();
        private bool _lastWasString = false;
        private BytecodeGenerator _generator;

        public List<byte> Bytecode
        {
            get { return _bytecode; }
        }

        public BytecodeSegment(BytecodeGenerator generator, IEnumerable<byte> bytecode = null)
        {
            if(bytecode != null)
                _bytecode = bytecode.ToList();
            _generator = generator;
        }

        /// <summary>
        /// Add an opcode without any special treatment.
        /// </summary>
        /// <param name="opcode">The opcode to add.</param>
        /// <param name="data">The data for this opcode.</param>
        public void AddGeneric(RantOpCode opcode, IEnumerable<byte> data)
        {
            _bytecode.Add((byte)opcode);
            _bytecode.AddRange(data);
            _lastWasString = false;
        }

        /// <summary>
        /// Add an opcode that references a string in the string table.
        /// </summary>
        /// <param name="opcode">The opcode to add.</param>
        /// <param name="str">The string.</param>
        public void AddStringReference(RantOpCode opcode, string str)
        {
            // last was a string constant, so merge the two
            if(_lastWasString)
            {
                var lastItem = _stringReferences.Last();
                var lastValue = _generator.GetString(lastItem.Value);
                lastValue += str;
                _stringReferences[lastItem.Key] = _generator.ReplaceIfUnused(lastItem.Value, lastValue);
                return;
            }
            var reference = _generator.AddString(str);
            _bytecode.Add((byte)opcode);
            _stringReferences[_bytecode.Count - 1] = reference;
            _bytecode.AddRange(BitConverter.GetBytes(reference));
            _lastWasString = true;
        }

        /// <summary>
        /// Mark the current position in the bytecode, if this is a debug build.
        /// </summary>
        /// <param name="stringe">The current stringe.</param>
        public void MarkPosition(Stringe stringe)
        {
            MarkPosition(stringe.Line, stringe.Column, stringe.Offset);
        }

        /// <summary>
        /// Mark the current position in the bytecode, if this is a debug build.
        /// </summary>
        /// <param name="line">The current line.</param>
        /// <param name="column">The current column.</param>
        /// <param name="index">The current index.</param>
        public void MarkPosition(int line, int column, int index)
        {
            if(!_generator.Debug) return;
            _bytecode.Add((byte)RantOpCode.Debug);
            _bytecode.AddRange(BitConverter.GetBytes(line));
            _bytecode.AddRange(BitConverter.GetBytes(column));
            _bytecode.AddRange(BitConverter.GetBytes(index));
        }
    }
}
