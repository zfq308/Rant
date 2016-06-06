using System;

namespace Rant
{
	/// <summary>
	/// Represents a compiled Rant program.
	/// </summary>
	public sealed class RantProgram
	{
		private readonly byte[] _bytecode;

		internal RantProgram(byte[] bytecode)
		{
			_bytecode = bytecode;
		}

		internal byte[] Bytecode => _bytecode;

		public static RantProgram LoadProgramFromFile(string path)
		{
			throw new NotImplementedException();
		}
	}
}