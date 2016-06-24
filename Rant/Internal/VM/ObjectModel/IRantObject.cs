using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Rant.Internal.VM.ObjectModel
{
	/// <summary>
	/// Represents a Rant variable.
	/// </summary>
	public interface IRantObject
	{
		/// <summary>
		/// The value of the object.
		/// </summary>
		object Value { get; }

		/// <summary>
		/// Returns another IRantObject instance with the exact same value as the current instance.
		/// </summary>
		/// <returns></returns>
		IRantObject Clone();
		IRantObject Add(IRantObject b);
		IRantObject Subtract(IRantObject b);
		IRantObject Multiply(IRantObject b);
		IRantObject Divide(IRantObject b);
		IRantObject Mod(IRantObject b);
		IRantObject Negative();
		IRantObject Concat(IRantObject b);
		IRantObject Increment();
		IRantObject Decrement();

		/// <summary>
		/// Returns a string representation of the current IRantObject.
		/// </summary>
		/// <returns></returns>
		string ToString();

		void Compare(IRantObject b, out int result, out bool comparable);
	}
}