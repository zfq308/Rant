namespace Rant.Internal.VM.Instructions
{
	internal enum OpCodes
	{
		/// <summary>
		/// Debug-mode data provider. Sets the current position within the source code at which the next instruction takes place.
		/// This instruction should ONLY be emitted during debug-mode compilation.
		/// <para>dbg [int line] [int col] [int index]</para>
		/// </summary>
		Debug = 0x00,
		/// <summary>
		/// Retrieves the string at the specified index from the string table and prints it to the output.
		/// <para>pstrc [int table_index]</para>
		/// </summary>
		PrintStringConstant = 0x01,
		/// <summary>
		/// Prints the specified number to the output.
		/// <para>pnumc [double number]</para>
		/// </summary>
		PrintNumberConstant = 0x02,
		/// <summary>
		/// Opens a channel with a name equal to the string at the specified string table index and with a visibility corresponding to an enum value from the ChannelVisibility enum.
		/// <para>open [int table_index] [byte visibility]</para>
		/// </summary>
		OpenChannelConstant = 0x03,
		/// <summary>
		/// Closes a channel with a name equal to the string at the specified string table index.
		/// <para>close [int table_index]</para>
		/// </summary>
		CloseChannelConstant = 0x04,
		/// <summary>
		/// Executes a block using the offset array provided at the specified block data table index.
		/// <para>block [int block_table_index]</para>
		/// </summary>
		Block = 0x05,
		/// <summary>
		/// Signifies the end of a block element and instructs the VM to continue on the next repeater cycle, if one is active.
		/// <para>bend</para>
		/// </summary>
		BlockEnd = 0x06,
		/// <summary>
		/// Prints the topmost item on the stack and consumes it.
		/// <para>print</para>
		/// </summary>
		Print = 0x07,
		/// <summary>
		/// Prints a dynamic escape sequence (one with randomness).
		/// <para>esc [byte escape_code] [int quantity]</para>
		/// </summary>
		PrintChars = 0x08,
		/// <summary>
		/// Calls the specified function. Arguments should be pushed to the stack.
		/// <para>call [short func_code]</para>
		/// </summary>
		Call = 0x09
	}
}