using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using Rant.Internal.VM;
using Rant.Internal.VM.Compiler;

namespace Rant
{
	/// <summary>
	/// Represents a compiled Rant program.
	/// </summary>
	public sealed class RantProgram
	{
		private readonly byte[] _bytecode;
		private readonly RantProgramData _data;
		private readonly string _name;

		public string Name => _name;

        public byte[] Bytecode => _bytecode;

        public RantProgramData Data => _data;

        internal RantProgram(string name, byte[] bytecode, RantProgramData data)
		{
			_name = name;
			_bytecode = bytecode;
			_data = data;
		}

		public static RantProgram LoadProgramFile(string path)
		{
            return LoadProgram(Path.GetFileName(path), File.ReadAllBytes(path));
		}

        public static RantProgram LoadProgram(string name, byte[] data)
        {
            var reader = new BinaryReader(new MemoryStream(data));
            if(string.Join("", reader.ReadChars(4)) != RantCompiler.MAGIC_NUMBER)
            {
                throw new InvalidDataException("Not a valid Rant program.");
            }
            var sectionCount = reader.ReadByte();
            var sectionAddresses = new Dictionary<byte, int>();
            for(var i = 0; i < sectionCount; i++)
            {
                sectionAddresses[reader.ReadByte()] = reader.ReadInt32();
            }

            // read bytecode
            if(!sectionAddresses.ContainsKey(RantCompiler.CODE_SECTION_ID))
            {
                throw new InvalidDataException("Program doesn't contain code section.");
            }
            reader.BaseStream.Position = sectionAddresses[RantCompiler.CODE_SECTION_ID];
            var bytecodeLength = reader.ReadInt32();
            var bytecode = reader.ReadBytes(bytecodeLength);

            // read string table
            if(!sectionAddresses.ContainsKey(RantCompiler.STRING_TABLE_SECTION_ID))
            {
                throw new InvalidDataException("Program doesn't contain string table section.");
            }
            reader.BaseStream.Position = sectionAddresses[RantCompiler.STRING_TABLE_SECTION_ID];
            var tableCount = reader.ReadInt32();
            var table = new string[tableCount];
            for(var i = 0; i < tableCount; i++)
            {
                var stringLength = reader.ReadInt32();
                var str = Encoding.UTF8.GetString(reader.ReadBytes(stringLength));
                table[i] = str;
            }
            var programData = new RantProgramData(table, new Regex[0], new BlockData[0]);
            return new RantProgram(name, bytecode, programData);
        }

		public static RantProgram CompileString(string source)
		{
			throw new NotImplementedException();
		}

		public static RantProgram CompileFile(string path)
		{
			throw new NotImplementedException();
		}
	}
}