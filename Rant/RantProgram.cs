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

		internal RantProgram(byte[] bytecode, RantProgramData data)
		{
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