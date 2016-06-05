using System.Collections.Generic;

using Rant.Internal.VM.Instructions;

namespace Rant.Internal.VM.Compiler
{
	internal class RantCompiler
	{
		private readonly Dictionary<string, int> stringTableMap = new Dictionary<string, int>();
		private int nextStringIndex = 0;
		private readonly List<Op> _ops = new List<Op>(32);



		public int GetStringIndex(string value)
		{
			if (value == null) return 0;
			int index;
			if (stringTableMap.TryGetValue(value, out index)) return index;
			return stringTableMap[value] = nextStringIndex++;
		}
	}
}