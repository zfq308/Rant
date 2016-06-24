using System;
using System.Collections.Generic;

using Rant.Internal.VM.ObjectModel;

namespace Rant.Internal.VM
{
	internal class Frame
	{
		// TODO: Query args
		public string QueryTableName = String.Empty;
		public int Position = 0;
		public readonly int StackStartIndex;
		public readonly int StackArgsLength;
		private readonly RVM _vm;
		private readonly RantProgram _assembly;

		public RantProgram Assembly => _assembly;

		public Frame(RVM vm, RantProgram assembly, int argc, int position)
		{
			_vm = vm;
			if (vm.Stack.Size < argc)
				vm.RuntimeError($"Insufficient stack size for function call (Expected at least {argc}, got {stack.Size}).");
			StackArgsLength = argc;
			StackStartIndex = vm.Stack.Size;
			Position = position;
			_assembly = assembly;
		}

		public RantObject GetArg(int argIndex)
		{
			if (argIndex >= StackArgsLength)
				_vm.RuntimeError($"Access violation at 0x{Position:X8}: Argument access requested an index that doesn't exist.");

			return _vm.Stack.
		}
	}
}