using Rant.Internal.IO;
using Rant.Internal.VM.Compiler;

namespace Rant.Internal.VM.Instructions
{
	[OpCode(0x02)]
	internal class OpReturn : Op
	{
		public override void ReadData(RantProgramData data, EasyReader reader)
		{	
			// Nothing to read!
		}

		public override void WriteData(RantCompiler compiler, EasyWriter writer)
		{
			// Nothing to write!
		}

		public override string GetOpString() => "ret";

		public override void Run(VM vm)
		{
			
		}
	}
}