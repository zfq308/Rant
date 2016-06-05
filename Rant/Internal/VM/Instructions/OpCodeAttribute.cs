using System;

namespace Rant.Internal.VM.Instructions
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	internal sealed class OpCodeAttribute : Attribute
	{
		public byte Code { get; set; }

		public OpCodeAttribute(byte code)
		{
			Code = code;
		}
	}
}