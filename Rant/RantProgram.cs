using System;

using Rant.Internal.VM.Instructions;

namespace Rant
{
	/// <summary>
	/// Represents a compiled Rant program.
	/// </summary>
	public sealed class RantProgram
	{
		private readonly Op[] _bytecode;

		internal RantProgram(Op[] bytecode)
		{
			_bytecode = bytecode;
		}

		internal Op[] Bytecode => _bytecode;

		public static RantProgram LoadProgramFromFile(string path)
		{
			throw new NotImplementedException();
		}
	}
}