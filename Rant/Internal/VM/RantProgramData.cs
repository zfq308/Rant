using System;

namespace Rant.Internal.VM
{
	internal class RantProgramData
	{
		private readonly string[] _stringTable;
		private readonly BlockData[] _blockDataTable;

		public RantProgramData(string[] stringTable, BlockData[] blockDataTable)
		{
			_stringTable = stringTable;
			_blockDataTable = blockDataTable;
		}

		public string GetString(int index)
		{
			if (index < 0 || index >= _stringTable.Length) return String.Empty;
			return _stringTable[index];
		}

		public BlockData GetBlockData(int index)
		{
			if (index < 0 || index >= _blockDataTable.Length) return null;
			return _blockDataTable[index];
		}
	}
}