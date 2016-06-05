using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Rant.Internal.Engine;

namespace Rant
{
	public sealed class SeedPool : IEnumerable<long>
	{
		private readonly long[] pool;
		private readonly int _size;

		public string Name { get; }

		public int Size => _size;

		private SeedPool(string name, RNG rng, int size)
		{
			Name = name;
			_size = size;
			pool = new long[size];
			for (int i = 0; i < size; i++)
			{
				pool[i] = rng.NextRaw();
			}
		}

		public static SeedPool FromSeed(string name, long seed, int size)
		{
			if (Util.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty or contain only whitespace characters.", nameof(name));
			if (size <= 0) throw new ArgumentException("Size must be greater than zero.", nameof(size));
			return new SeedPool(name, new RNG(seed), size);
		}

		public static SeedPool FromRNG(string name, RNG rng, int size)
		{
			if (Util.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty or contain only whitespace characters.", nameof(name));
			if (size <= 0) throw new ArgumentException("Size must be greater than zero.", nameof(size));
			if (rng == null) throw new ArgumentNullException(nameof(rng));
			return new SeedPool(name, rng, size);
		}

		public long this[int index] => pool[index];

		public RNG this[string index] => new RNG(pool[Math.Abs(index.Hash()) % _size]);

		public long GetNext(RNG rng) => pool[rng.Next(_size)];

		public IEnumerator<long> GetEnumerator()
		{
			foreach (var seed in pool) yield return seed;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}