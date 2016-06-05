using System;

using Rant.Internal.IO;
using Rant.Internal.VM.Compiler;

namespace Rant.Internal.VM.Instructions
{
	[OpCode(0x01)]
	internal class OpPrintString : Op
	{
		private string _value = String.Empty;

		public string Value
		{
			get { return _value; }
			set { _value = value ?? String.Empty; }
		}

		public override void ReadData(RantProgramData data, EasyReader reader)
		{
			_value = data.GetString(reader.ReadInt32());
		}

		public override void WriteData(RantCompiler compiler, EasyWriter writer)
		{
			writer.Write(compiler.GetStringIndex(_value));
		}

		public override string GetOpString() => $"pstr \"{_value}\"";

		public override void Run(VM vm)
		{
			vm.Print(_value);
		}
	}
}