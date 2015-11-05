using System;
using System.Collections.Generic;
using System.Linq;

namespace Rant.Vocabulary
{
	internal class Sieve<T>
	{
		private readonly T[] _mainArray;
		private T[] _a;
		private T[] _b;
		private int _size;

		public Sieve(IEnumerable<T> collection)
		{
			if (collection == null) throw new ArgumentNullException(nameof(collection));
			_mainArray = collection.ToArray();
			_size = _mainArray.Length;
			_a = new T[_mainArray.Length];
			_b = new T[_mainArray.Length];
			Array.Copy(_mainArray, _a, _mainArray.Length);
		}

		public int OriginalLength => _mainArray.Length;

		public int Length => _size;

		public bool Filter(Func<T, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			int n = 0;
			// Filter operations sift passing items from one array to the other.
			// Then the destination array is the one that is indexed.
			// The arrays are swapped after each filter.
			for (int i = 0; i < _size; i++)
			{
				if (predicate(_a[i])) _b[n++] = _a[i];
			}
			_size = n;
			var swap = _b;
			_b = _a;
			_a = swap;
			return _size > 0;
		}

		public void Reset() => _size = _mainArray.Length;

		public T this[int i]
		{
			get
			{
				if (i < 0) throw new ArgumentOutOfRangeException(nameof(i), "Index must be non-negative and less than the size of the collection.");
				if (i >= _size) throw new ArgumentOutOfRangeException(nameof(i), "Index must be non-negative and less than the size of the collection.");
				return _size == _mainArray.Length ? _mainArray[i] : _a[i];
			}
		}
	}
}