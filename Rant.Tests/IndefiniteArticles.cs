using Rant;
using NUnit.Framework;

namespace Rant.Tests
{
	[TestFixture]
	public class IndefiniteArticles
	{
		private RantEngine _engine = new RantEngine();

		[Test]
		[TestCase("boy")]
		[TestCase("car")]
		[TestCase("bike")]
		[TestCase("man")]
		[TestCase("rat")]
		[TestCase("cat")]
		[TestCase("zoo")]
		public void Consonant(string word)
		{
			Assert.AreEqual("a " + word, _engine.Do(@"\a " + word).Main);
		}

		[Test]
		[TestCase("honor")]
		[TestCase("hour")]
		[TestCase("honour")]
		[TestCase("heir")]
		[TestCase("hourglass")]
		public void SilentH(string word)
		{
			Assert.AreEqual("an " + word, _engine.Do(@"\a " + word).Main);
		}

		[Test]
		[TestCase("hotel")]
		[TestCase("horrible")]
		[TestCase("hospital")]
		[TestCase("humorous")]
		[TestCase("human")]
		[TestCase("host")]
		public void FalseSilentH(string word)
		{
			Assert.AreEqual("a " + word, _engine.Do(@"\a " + word).Main);
		}

		[Test]
		[TestCase("elephant")]
		[TestCase("egg")]
		[TestCase("apple")]
		[TestCase("orphan")]
		[TestCase("idiot")]
		[TestCase("unanticipated")]
		[TestCase("unhappy")]
		[TestCase("underground")]
		public void Vowel(string word)
		{
			Assert.AreEqual("an " + word, _engine.Do(@"\a " + word).Main);
		}

		[Test]
		[TestCase("user")]
		[TestCase("unanimous")]
		[TestCase("university")]
		[TestCase("unicycle")]
		[TestCase("union")]
		[TestCase("unit")]
		[TestCase("unique")]
		public void YSound(string word)
		{
			Assert.AreEqual("a " + word, _engine.Do(@"\a " + word).Main);
		}

		[Test]
		[TestCase("men")]
		[TestCase("dogs")]
		[TestCase("fungi")]
		[TestCase("octopuses")]
		[TestCase("viruses")]
		[TestCase("geese")]
		[TestCase("mice")]
		[TestCase("people")]
		[TestCase("cacti")]
		[TestCase("teeth")]
		public void Plurals(string word)
		{
			Assert.AreEqual("the " + word, _engine.Do(@"\a " + word).Main);
		}
	}
}
