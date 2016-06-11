using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rant.Internal.VM.Instructions;

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

        private List<BytecodeSegment> _program;

        public BytecodeSegment LatestSegment
        {
            get { return _program.Last(); }
        }

        public BytecodeGenerator()
        {
            _stringTable = new List<string>();
            _stringTableSet = new HashSet<string>();
            _program = new List<BytecodeSegment> { new BytecodeSegment() };
        }

        public byte[] Compile()
        {
            return _program.Last().Bytecode.ToArray();
        }

        public int AddString(string value)
        {
            if(_stringTableSet.Contains(value))
                return _stringTable.IndexOf(value);
            _stringTableSet.Add(value);
            _stringTable.Add(value);
            return _stringTable.Count - 1;
        }

        public void AddSegment(BytecodeSegment segment)
        {
            _program.Add(segment);
        }
    }

    internal class BytecodeSegment
    {
        private List<byte> _bytecode = new List<byte>();

        public List<byte> Bytecode
        {
            get { return _bytecode; }
        }

        public BytecodeSegment(IEnumerable<byte> bytecode = null)
        {
            if(bytecode != null)
                _bytecode = bytecode.ToList();
        }

        public void AddGeneric(OpCodes opcode, IEnumerable<byte> data)
        {
            _bytecode.Add((byte)opcode);
            _bytecode.AddRange(data);
        }
    }
}
