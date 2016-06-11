using System;
using System.Collections.Generic;

namespace Rant.Internal.VM
{
	internal class CallFrame
	{
		// TODO: Query args
		public string QueryTableName = String.Empty;
		public int Position = 0;

		public CallFrame(int position)
		{
			Position = position;
		}
	}
}