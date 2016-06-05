using System;

namespace Rant.Internal.VM
{
	internal class RantProgramData
	{
		private readonly string[] _stringTable;

		public RantProgramData(string[] stringTable)
		{
			_stringTable = stringTable;
		}

		public string GetString(int index)
		{
			if (index < 0 || index >= _stringTable.Length) return String.Empty;
			return _stringTable[index];
		}
	}
}