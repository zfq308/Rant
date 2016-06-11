namespace Rant.Internal.VM
{
	public class BlockData
	{
		public int EndAddress;
		public Item[] Items;
		

		public BlockData(int endAddress, Item[] items)
		{
			EndAddress = endAddress;
			Items = items;
		}

		public struct Item
		{
			public int Offset;
			public ItemWeightType WeightType;
			public float Weight;
		}

		public enum ItemWeightType : byte
		{
			None = 0x00,
			Constant = 0x01,
			Interpreted = 0x02
		}
	}
}