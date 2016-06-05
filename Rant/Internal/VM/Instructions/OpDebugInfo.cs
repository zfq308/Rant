using Rant.Internal.IO;
using Rant.Internal.VM.Compiler;

namespace Rant.Internal.VM.Instructions
{
	[OpCode(0x00)]
	internal class OpDebugInfo : Op
	{
		public int Line { get; } = 1;
		public int Column { get; } = 1;
		public int CharIndex { get; } = 0;

		public override void ReadData(EasyReader reader)
		{
			throw new System.NotImplementedException();
		}

		public override void WriteData(RantCompiler compiler, EasyWriter writer)
		{
			throw new System.NotImplementedException();
		}

		public override string GetOpString() => $"(Line {Line}, Col {Column}, Index {CharIndex})";

		public override void Run(VM vm)
		{
			throw new System.NotImplementedException();
		}
	}
}