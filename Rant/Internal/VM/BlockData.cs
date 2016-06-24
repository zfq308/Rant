namespace Rant.Internal.VM
{
	/// <summary>
	/// Stores information, such as jump locations and weighting, for a specific block in a compiled pattern.
	/// </summary>
	public class BlockData
	{
		/// <summary>
		/// The address directly after the block ends.
		/// </summary>
		public int EndAddress;

		/// <summary>
		/// The items in the block.
		/// </summary>
		public Item[] Items;
		

		public BlockData(int endAddress, Item[] items)
		{
			EndAddress = endAddress;
			Items = items;
		}

		public struct Item
		{
			/// <summary>
			/// The address of the beginning of the block item code.
			/// </summary>
			public int Offset;
			/// <summary>
			/// The type of weight used by the item.
			/// </summary>
			public ItemWeightType WeightType;
			/// <summary>
			/// If the weight is constant, this field contains the value.
			/// </summary>
			public float Weight;
			/// <summary>
			/// If the weight is interpreted, this field contains the offset to the weight pattern.
			/// </summary>
			public int InterpretOffset;
		}

		public enum ItemWeightType : byte
		{
			/// <summary>
			/// No weight is used.
			/// </summary>
			None = 0x00,
			/// <summary>
			/// The weight is a constant value.
			/// </summary>
			Constant = 0x01,
			/// <summary>
			/// The weight is determined by a pattern every time the parent block is encountered.
			/// </summary>
			Interpreted = 0x02
		}
	}
}