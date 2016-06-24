namespace Rant.Internal.VM.ObjectModel
{
	public sealed class RantFunction : IRantObject
	{
		public object Value { get; }

		public IRantObject Clone()
		{
			throw new System.NotImplementedException();
		}

		public IRantObject Add(IRantObject b)
		{
			throw new System.NotImplementedException();
		}

		public IRantObject Subtract(IRantObject b)
		{
			throw new System.NotImplementedException();
		}

		public IRantObject Multiply(IRantObject b)
		{
			throw new System.NotImplementedException();
		}

		public IRantObject Divide(IRantObject b)
		{
			throw new System.NotImplementedException();
		}

		public IRantObject Mod(IRantObject b)
		{
			throw new System.NotImplementedException();
		}

		public IRantObject Negative()
		{
			throw new System.NotImplementedException();
		}

		public IRantObject Concat(IRantObject b)
		{
			throw new System.NotImplementedException();
		}

		public IRantObject Increment()
		{
			throw new System.NotImplementedException();
		}

		public IRantObject Decrement()
		{
			throw new System.NotImplementedException();
		}

		public void Compare(IRantObject b, out int result, out bool comparable)
		{
			throw new System.NotImplementedException();
		}
	}
}