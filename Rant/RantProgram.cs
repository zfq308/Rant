using System;

using Rant.Internal.VM;

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

		internal RantProgram(string name, byte[] bytecode, RantProgramData data)
		{
			_name = name;
			_bytecode = bytecode;
			_data = data;
		}

		internal byte[] Bytecode => _bytecode;

		internal RantProgramData Data => _data;

		public static RantProgram LoadProgramFile(string path)
		{
			throw new NotImplementedException();
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