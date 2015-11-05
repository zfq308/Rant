using System;
using System.Collections.Generic;
using System.Linq;

namespace Rant.Vocabulary
{
	internal class Sieve<T>
	{
		private readonly T[] _mainArray;
		private readonly T[] _a;
		private readonly T[] _b;
		private bool stage = true;
		private int _sizeA, _sizeB;

		public Sieve(IEnumerable<T> collection)
		{
			if (collection == null) throw new ArgumentNullException(nameof(collection));
			_mainArray = collection.ToArray();
			Array.Copy(_mainArray, _a, _mainArray.Length);
			_sizeA = _mainArray.Length;
			_a = new T[_mainArray.Length];
			_b = new T[_mainArray.Length];
		}

		public int OriginalLength => _mainArray.Length;

		public int Length => stage ? _sizeA : _sizeB;

		public bool Filter(Func<T, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			int n = 0;
			if (stage)
			{
				for (int i = 0; i < _sizeA; i++)
				{
					if (predicate(_a[i])) _b[n++] = _a[i];
				}
				_sizeB = n;
			}
			else
			{
				for (int i = 0; i < _sizeB; i++)
				{
					if (predicate(_b[i])) _a[n++] = _b[i];
				}
				_sizeA = n;
			}
			stage = !stage;
			return Length > 0;
		}

		public void Reset()
		{
			stage = true;
			_sizeA = _mainArray.Length;
		}

		public T this[int i]
		{
			get
			{
				if (i < 0) throw new ArgumentOutOfRangeException(nameof(i), "Index must be non-negative and less than the size of the collection.");
				if (stage)
				{
					if (i >= _sizeA)
						throw new ArgumentOutOfRangeException(nameof(i), "Index must be non-negative and less than the size of the collection.");
					return _sizeA == _mainArray.Length ? _mainArray[i] : _a[i];
				}
				if (i >= _sizeB)
					throw new ArgumentOutOfRangeException(nameof(i), "Index must be non-negative and less than the size of the collection.");
				return _sizeB == _mainArray.Length ? _mainArray[i] : _b[i];
			}
		}
	}
}