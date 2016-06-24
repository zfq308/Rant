using System;

namespace Rant.Internal.VM.ObjectModel
{
	public sealed class RantUndefined : IRantObject
	{
		public static readonly RantUndefined Instance = new RantUndefined();

		private RantUndefined()
		{
			if (Instance != null)
				throw new InvalidOperationException("I'm sorry, Dave, I'm afraid I can't do that.");
		}

		public object Value => null;

		public IRantObject Clone() => this;

		public IRantObject Add(IRantObject b) => this;

		public IRantObject Subtract(IRantObject b) => this;

		public IRantObject Multiply(IRantObject b) => this;

		public IRantObject Divide(IRantObject b) => this;

		public IRantObject Mod(IRantObject b) => this;

		public IRantObject Negative() => this;

		public IRantObject Concat(IRantObject b) => this;

		public IRantObject Increment() => this;

		public IRantObject Decrement() => this;

		public override string ToString() => "???";

		public void Compare(IRantObject b, out int result, out bool comparable)
		{
			result = 0;
			comparable = false;
		}
	}
}