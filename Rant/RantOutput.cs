﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Rant.Internal.VM.Output;

namespace Rant
{
    /// <summary>
    /// Represents a collection of strings generated by a pattern.
    /// </summary>
    public sealed class RantOutput : IEnumerable<RantOutputEntry>
    {
        private readonly Dictionary<string, RantOutputEntry> _outputs;

        internal RantOutput(long seed, long startingGen, IEnumerable<OutputChain> chains)
        {
            _outputs = chains.ToDictionary(chain => chain.Name, chain => new RantOutputEntry(chain.Name, chain.ToString(), chain.Visibility));
            Seed = seed;
            BaseGeneration = startingGen;
        }

        internal RantOutput(long seed, long startingGen, IEnumerable<Rant.Internal.Engine.Output.OutputChain> chains)
        {
            _outputs = chains.ToDictionary(chain => chain.Name, chain => new RantOutputEntry(chain.Name, chain.ToString(), chain.Visibility));
            Seed = seed;
            BaseGeneration = startingGen;
        }

        /// <summary>
        /// Gets the output of the channel with the specified name.
        /// </summary>
        /// <param name="channel">The name of the channel.</param>
        /// <returns></returns>
        public string this[string channel]
        {
            get
            {
                RantOutputEntry value;
                return _outputs.TryGetValue(channel, out value) ? value.Value : String.Empty;
            }
        }

        /// <summary>
        /// Gets an array containing the values of the specified channels, in the order they appear.
        /// </summary>
        /// <param name="channels">The names of the channels whose values are to be retrieved.</param>
        /// <returns></returns>
        public string[] this[params string[] channels]
        {
            get
            {
                if (channels == null || channels.Length == 0) return new string[0];
                var output = new string[channels.Length];
                for (int i = 0; i < channels.Length; i++)
                {
                    if (channels[i] == null)
                    {
                        output[i] = String.Empty;
                        continue;
                    }
                    RantOutputEntry value;
                    if (_outputs.TryGetValue(channels[i], out value))
                    {
                        channels[i] = value.Value;
                    }
                    else
                    {
                        channels[i] = String.Empty;
                    }
                }
                return channels;
            }
        }

        /// <summary>
        /// The seed used to generate the output.
        /// </summary>
        public long Seed { get; }

        /// <summary>
        /// The generation at which the RNG was initially set before the pattern was run.
        /// </summary>
        public long BaseGeneration { get; }

        /// <summary>
        /// The main output string.
        /// </summary>
        public string Main => _outputs["main"].Value;

        /// <summary>
        /// Returns an enumerator that iterates through the outputs in the collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<RantOutputEntry> GetEnumerator() => _outputs.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _outputs.Values.GetEnumerator();

        /// <summary>
        /// Returns the output from the "main" channel.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Main;

        /// <summary>
        /// Returns the output from the "main" channel.
        /// </summary>
        /// <returns></returns>
        public static implicit operator string(RantOutput output) => output.Main;
    }
}