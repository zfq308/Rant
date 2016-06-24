using System;

namespace Rant.Internal.VM.ObjectModel
{
	public sealed class RantString : IRantObject
	{
		private string _value = String.Empty;

		public RantString(string value)
		{
			_value = value ?? String.Empty;
		}

		public object Value => _value;

		public IRantObject Clone() => new RantString(_value);

		public IRantObject Add(IRantObject b) => new RantString(String.Concat(_value, b.Value));

		public IRantObject Subtract(IRantObject b) => RantUndefined.Instance;

		public IRantObject Multiply(IRantObject b) => RantUndefined.Instance;

		public IRantObject Divide(IRantObject b) => RantUndefined.Instance;

		public IRantObject Mod(IRantObject b) => RantUndefined.Instance;

		public IRantObject Negative() => RantUndefined.Instance;

		public IRantObject Concat(IRantObject b) => new RantString(String.Concat(_value, b.Value));

		public IRantObject Increment() => RantUndefined.Instance;

		public IRantObject Decrement() => RantUndefined.Instance;

		public void Compare(IRantObject b, out int result, out bool comparable)
		{
			var bs = b as RantString;
			result = 0;
			if (bs == null)
			{
				comparable = false;
				return;
			}
			comparable = String.Equals(_value, bs._value, StringComparison.InvariantCulture);
		}
	}
}