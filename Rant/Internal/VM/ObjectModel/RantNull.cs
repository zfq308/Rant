using System;

namespace Rant.Internal.VM.ObjectModel
{
	public sealed class RantNull : IRantObject
	{
		public static readonly RantNull Instance = new RantNull();

		private RantNull()
		{
			if (Instance != null)
				throw new InvalidOperationException("I'm sorry Dave, I'm afraid I can't do that.");
		}

		public object Value => null;

		public IRantObject Clone() => this;
		public IRantObject Add(IRantObject b) => null;

		public IRantObject Subtract(IRantObject b) => null;

		public IRantObject Multiply(IRantObject b) => null;

		public IRantObject Divide(IRantObject b) => null;

		public IRantObject Mod(IRantObject b) => null;

		public IRantObject Negative() => null;

		public IRantObject Concat(IRantObject b) => null;

		public IRantObject Increment() => this;

		public IRantObject Decrement() => this;

		public override string ToString() => "null";

		public void Compare(IRantObject b, out int result, out bool comparable)
		{
			result = 0;
			comparable = ReferenceEquals(this, b);
		}
	}
}