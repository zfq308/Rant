using System.Collections.Generic;

using Rant.Vocabulary;

namespace Rant
{
	/// <summary>
	/// Represents a pool of resources that can be accessed and modified by one or more RantEngine instances.
	/// </summary>
	public sealed class RantResourceContainer
	{
		private readonly Dictionary<string, RantPackage> _packages = new Dictionary<string, RantPackage>();
		private readonly RantDictionary _dictionary = new RantDictionary();
		private readonly Dictionary<string, RantPattern> _patternMap = new Dictionary<string, RantPattern>(); 

		private class PatternDirectory
		{
			private readonly string _name;
			private readonly Dictionary<string, PatternDirectory> _subdirs;
			private readonly Dictionary<string, RantPattern> _patterns;

			public string Name => _name;
		}
	}
}