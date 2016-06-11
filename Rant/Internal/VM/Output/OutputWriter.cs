﻿using System;
using System.Collections.Generic;

using Rant.Internal.Engine.Formatters;

namespace Rant.Internal.VM.Output
{
	internal class OutputWriter
	{
		private readonly RVM _vm;
		private readonly OutputChain mainChain;
		private readonly Dictionary<string, OutputChain> chains = new Dictionary<string, OutputChain>(); 
		private readonly Stack<OutputChain> chainStack = new Stack<OutputChain>();
		private readonly HashSet<OutputChain> activeChains = new HashSet<OutputChain>();

		private const string MainChannelName = "main";

		public OutputWriter(RVM vm)
		{
			_vm = vm;
			mainChain = chains[MainChannelName] = new OutputChain(_vm, MainChannelName);
			chainStack.Push(mainChain);
			activeChains.Add(mainChain);
		}

		public bool CloseChannel()
		{
			if (chainStack.Peek() == mainChain) return false;
			activeChains.Remove(chainStack.Pop());
			return true;
		}

		public void OpenChannel(string name, ChannelVisibility visibility)
		{
			OutputChain chain;
			if (!chains.TryGetValue(name, out chain))
			{
				chain = chains[name] = new OutputChain(_vm, name);
			}
			else if (activeChains.Contains(chain))
			{
				return;
			}
			chain.Visibility = visibility;
			chainStack.Push(chain);
			activeChains.Add(chain);
		}

		public void Do(Action<OutputChain> chainAction)
		{
			bool fInternal = false;
			foreach (var chain in chainStack)
			{
				if (fInternal && chain == mainChain) return;
				chainAction(chain);
				switch (chain.Visibility)
				{
					case ChannelVisibility.Public:
						if (fInternal) return;
						if (chain != mainChain)
						{
							chainAction(mainChain);
						}
						return;
					case ChannelVisibility.Private:
						return;
					case ChannelVisibility.Internal:
						fInternal = true;
						break;
				}
			}
		}

		public void Capitalize(Capitalization caps) => Do(chain => chain.Last.Caps = caps);

		public void Print(string value) => Do(chain => chain.Print(value));

		public void Print(object obj) => Do(chain => chain.Print(obj));

		public void InsertTarget(string targetName) => Do(chain => chain.InsertTarget(targetName));

		public void PrintToTarget(string targetName, string value) => Do(chain => chain.PrintToTarget(targetName, value));

		public RantOutput ToRantOutput() => new RantOutput(_vm.RNG.Seed, _vm.StartingGen, chains.Values);
	}
}