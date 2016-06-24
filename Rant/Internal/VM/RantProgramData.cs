using System;
using System.Text.RegularExpressions;

namespace Rant.Internal.VM
{
	public sealed class RantProgramData
	{
		private readonly string[] _stringTable;
		private readonly Regex[] _regexTable;
		private readonly BlockData[] _blockDataTable;
		private readonly RantProgram[] _references;

        /// <summary>
        /// The string table.
        /// </summary>
        public string[] StringTable => _stringTable;

		public RantProgramData(string[] stringTable, Regex[] regexTable, BlockData[] blockDataTable, RantProgram[] references)
		{
			_stringTable = stringTable;
			_blockDataTable = blockDataTable;
			_regexTable = regexTable;
			_references = references;
		}

		public string GetString(int index)
		{
			if (index < 0 || index >= _stringTable.Length) return String.Empty;
			return _stringTable[index];
		}

		public Regex GetRegex(int index)
		{
			if (index < 0 || index >= _regexTable.Length) return null;
			return _regexTable[index];
		}

		public BlockData GetBlockData(int index)
		{
			if (index < 0 || index >= _blockDataTable.Length) return null;
			return _blockDataTable[index];
		}

		public RantProgram GetReference(int index)
		{
			if (index < 0 || index >= _references.Length) return null;
			return _references[index];
		}
	}
}