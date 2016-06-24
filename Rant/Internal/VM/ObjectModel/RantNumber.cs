using System;
using System.Globalization;

namespace Rant.Internal.VM.ObjectModel
{
	public sealed class RantNumber : IRantObject
	{
		private double _value;

		public RantNumber(double value)
		{
			_value = value;
		}

		public object Value => _value;

		public IRantObject Clone() => this;

		public IRantObject Add(IRantObject b)
		{
			var bn = b as RantNumber;
			if (bn != null) return new RantNumber(bn._value + _value);
			return RantUndefined.Instance;
		}

		public IRantObject Subtract(IRantObject b)
		{
			var bn = b as RantNumber;
			if (bn != null) return new RantNumber(bn._value - _value);
			return RantUndefined.Instance;
		}

		public IRantObject Multiply(IRantObject b)
		{
			var bn = b as RantNumber;
			if (bn != null) return new RantNumber(bn._value * _value);
			return RantUndefined.Instance;
		}

		public IRantObject Divide(IRantObject b)
		{
			var bn = b as RantNumber;
			if (bn != null) return new RantNumber(bn._value / _value);
			return RantUndefined.Instance;
		}

		public IRantObject Mod(IRantObject b)
		{
			var bn = b as RantNumber;
			if (bn != null) return new RantNumber(bn._value % _value);
			return RantUndefined.Instance;
		}

		public IRantObject Negative()
		{
			return new RantNumber(-_value);
		}

		public IRantObject Concat(IRantObject b)
		{
			return new RantString(String.Concat(_value.ToString(CultureInfo.InvariantCulture), b));
		}

		public IRantObject Increment()
		{
			_value++;
			return this;
		}

		public IRantObject Decrement()
		{
			_value--;
			return this;
		}

		public void Compare(IRantObject b, out int result, out bool comparable)
		{
			comparable = true;
			result = 0;
			var bn = b as RantNumber;
			if (bn == null)
			{
				comparable = false;
				return;
			}
			if (_value > bn._value)
			{
				result = 1;
			}
			else if (_value < bn._value)
			{
				result = -1;
			}
		}
	}
}