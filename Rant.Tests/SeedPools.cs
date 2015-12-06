using System.Linq;

using NUnit.Framework;

namespace Rant.Tests
{
	[TestFixture]
	public class SeedPools
	{
		private readonly RantEngine rant = new RantEngine();

		public SeedPools()
		{
			rant.AddSeedPool(SeedPool.FromSeed("thing", 0, 100));
		}

		[Test]
		public void InPatternSyncing()
		{
			var output = rant.DoSerial(@"[r:16]{[pool:thing;A;\32,x][yield]}").Select(o => o.Main).ToArray();
			Assert.AreEqual(16, output.Length);
			var first = output[0];
			for (int i = 1; i < 16; i++)
			{
				Assert.AreEqual(first, output[i], $"output[{i}] did not match output[0].");
			}
		}

		[Test]
		public void CrossPatternSyncing()
		{
			var pattern = RantPattern.FromString(@"[pool:thing;B;\32,x]");
			var first = rant.Do(pattern, seed: 0).Main;
			for (int i = 0; i < 16; i++)
			{
				var output = rant.Do(pattern, seed: i + 1);
				Assert.AreEqual(first, output.Main, $"Seed {i + 1} output did not match seed 0 output.");
			}
		}
	}
}