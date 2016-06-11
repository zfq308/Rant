using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rant.Engine.Output
{
	internal class OutputChainTypoBuffer : OutputChainBuffer
	{
		private Sandbox _sb;
		private TypoMode _mode = TypoMode.All;

		public OutputChainTypoBuffer(Sandbox sb, OutputChainBuffer prev) : base(sb, prev)
		{
			_sb = sb;
        }

		public OutputChainTypoBuffer(Sandbox sb, OutputChainBuffer prev, OutputChainBuffer targetOrigin) : base(sb, prev, targetOrigin)
		{
			_sb = sb;
        }

		protected override void OnBufferFormat(ref string value)
		{
			if (_mode == TypoMode.None) return;
			string[] words = value.Split(' ');
			for(int i = 0; i < words.Length; i++)
			{
				string word = words[i];
				// perform a random number of rounds on the word
				for (int j = 0; j < _sb.RNG.Next(1, 5); j++)
				{
					word = TypoRound(word);
				}
			}
			value = string.Join(" ", words);
		}

		private string TypoRound(string word)
		{
			if((_mode & TypoMode.Transpose) == TypoMode.Transpose)
			{

			}
		}

		[Flags]
		internal enum TypoMode : ushort
		{
			None = 0,
			// ie / ei, au / ua, ou / uo
			Transpose = 1,
			// using the wrong characters based on the adjacency of keys
			Mistype = 2,
			// accidentally mixing up the order of letters
			Order = 4,
			// collapsing adjacent characters
			Collapse = 8,
			// doubling consonants
			Double = 16,
			// common suffix errors
			Suffix = 32,
			// replacing letters with other letters they sound like
			Sound = 64,
			// capitalizing or uncapitalizing words
			Case = 128,
			// common misspellings that aren't generated under the other modes
			Common = 256,
			// running words together, separating words accidentally
			Spacing = 512,


			All = 0xffff
		}
	}
}
