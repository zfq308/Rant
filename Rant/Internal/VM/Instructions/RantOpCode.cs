namespace Rant.Internal.VM.Instructions
{
	internal enum RantOpCode : byte
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
		/// <para>close</para>
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
		/// Calls the specified native function. Arguments should be pushed to the stack.
		/// <para>ncall [short func_code]</para>
		/// </summary>
		NativeCall = 0x09,
		/// <summary>
		/// Returns to caller.
		/// <para>ret</para>
		/// </summary>
		Return = 0x0a,
		/// <summary>
		/// Creates a new channel stack, pushes it to the output stack, and sets it as the current printing destination.
		/// <para>newout</para>
		/// </summary>
		NewOutput = 0x0b,
		/// <summary>
		/// Pops the current channel stack and pushes its contents to the object stack.
		/// This is used by parameters for subroutines and built-in functions.
		/// <para>retout</para>
		/// </summary>
		ReturnOutput = 0x0c,
		/// <summary>
		/// Pushes a string constant onto the stack.
		/// <para>pushstr [int table_index]</para>
		/// </summary>
		PushString = 0x0d,
		/// <summary>
		/// Pushes a number constant onto the stack.
		/// <para>pushnum [double value]</para>
		/// </summary>
		PushNum = 0x0e,
		/// <summary>
		/// Pushes a subroutine pointer into the scope, marking it as available.
		/// <para>pushsub [int sub_id]</para>
		/// </summary>
		PushSub = 0x0f,
		/// <summary>
		/// Removes the specified amount of subroutines from the scope.
		/// <para>popsubs [int count]</para>
		/// </summary>
		PopSubs = 0x10,
		/// <summary>
		/// Calls the specified subroutine.
		/// <para>callsub [int sub_id] [int max_locals]</para>
		/// </summary>
		CallSub = 0x11,
		/// <summary>
		/// Assigns a string constant to a subroutine argument register.
		/// <para>argstr [int table_index] [int arg_id]</para>
		/// </summary>
		SubArgStringConstant = 0x12,
		/// <summary>
		/// Assigns a number constant to a subroutine argument register.
		/// <para>argnum [double value] [int arg_id]</para>
		/// </summary>
		SubArgNumberConstant = 0x13,
		/// <summary>
		/// Assigns a pointer constant to a subroutine argument register. Used for callbacks.
		/// <para>argptr [int ptr] [int arg_id]</para>
		/// </summary>
		SubArgPtr = 0x14,
		/// <summary>
		/// Pops the last object from the stack and assigns it to a subroutine argument register.
		/// <para>argobj [int arg_id]</para>
		/// </summary>
		PopToArgument = 0x15,
		/// <summary>
		/// Pushes the value of the specified argument to the stack.
		/// <para>pusharg [int arg_id]</para>
		/// </summary>
		PushArgument = 0x16,
		/// <summary>
		/// Redirects execution to the specified address.
		/// <para>jmp [int address]</para>
		/// </summary>
		Jump = 0x17,
		/// <summary>
		/// Pops the last two values off the stack and pushes their sum to the stack.
		/// <para>add</para>
		/// </summary>
		Add = 0x18,
		/// <summary>
		/// Pops the last two values off the stack, subtracts the topmost value from the one below it, and pushes the result to the stack.
		/// <para>sub</para>
		/// </summary>
		Subtract = 0x19,
		/// <summary>
		/// Pops the last two values off the stack and pushes their product to the stack.
		/// <para>mul</para>
		/// </summary>
		Multiply = 0x1a,
		/// <summary>
		/// Pops the last two values off the stack, divides the second value by the first one, and pushes the result to the stack.
		/// <para>div</para>
		/// </summary>
		Divide = 0x1b,
		/// <summary>
		/// Swaps the values of the two specified locals.
		/// <para>swap [int loc1] [int loc2]</para>
		/// </summary>
		Swap = 0x1c,
		/// <summary>
		/// Pops the last two values off the stack as strings, concatenates the first value to the second one, and pushes the resulting string to the stack.
		/// <para>concat</para>
		/// </summary>
		Concat = 0x1d,
		/// <summary>
		/// Creates a copy of the specified local and pushes it to the stack.
		/// <para>ldloc [int index]</para>
		/// </summary>
		LoadLocal = 0x1e,
		/// <summary>
		/// Pops the topmost value off the stack and stores it in the specified local.
		/// <para>stloc [int index]</para>
		/// </summary>
		SetLocal = 0x1f,
		/// <summary>
		/// Pops a channel stack output off the stack and runs a replacement operation on it using the provided regex,
		/// replacing all matches with the output from the code at the pointer specified on the next item on the stack.
		/// The results are printed to the corresponding channels in the current output.
		/// <para>replace [int regex_index]</para>
		/// </summary>
		Replace = 0x20,
		/// <summary>
		/// Compares the last two items on the stack, bottom item first.
		/// <para>cmp</para>
		/// </summary>
		Compare = 0x21,
		/// <summary>
		/// Jumps to the specified address if A is equal to B.
		/// <para>jeq [int address]</para>
		/// </summary>
		JumpEqual = 0x22,
		/// <summary>
		/// Jumps to the specified address if A is not equal to B.
		/// <para>jne [int address]</para>
		/// </summary>
		JumpnotEqual = 0x23,
		/// <summary>
		/// Jumps to the specified address if A is greater than B.
		/// <para>jgt [int address]</para>
		/// </summary>
		JumpGreaterThan = 0x24,
		/// <summary>
		/// Jumps to the specified address if A is greater than or equal to B.
		/// <para>jge [int address]</para>
		/// </summary>
		JumpGreaterEqual = 0x25,
		/// <summary>
		/// Jumps to the specified address if A is less than B.
		/// <para>jlt [int address]</para>
		/// </summary>
		JumpLessThan = 0x26,
		/// <summary>
		/// Jumps to the specified address if A is less than or equal to B.
		/// <para>jle [int address]</para>
		/// </summary>
		JumpLessEqual = 0x27,
		/// <summary>
		/// Jumps to the specified address if A is equal to zero/false.
		/// <para>jz [int address]</para>
		/// </summary>
		JumpZero = 0x28,
		/// <summary>
		/// Jumps to the specified address if A is nonzero/true.
		/// <para>jnz [int address]</para>
		/// </summary>
		JumpNotZero = 0x29,
		/// <summary>
		/// Performs a modulo operation on the last two stack items and pushes the result.
		/// </summary>
		Modulo = 0x2a,
		/// <summary>
		/// Pushes zero to the stack.
		/// </summary>
		Zero = 0x2b,
		/// <summary>
		/// Pushes one to the stack.
		/// </summary>
		One = 0x2c,
		/// <summary>
		/// Pushes an address to the stack.
		/// <para>pushaddr [int address]</para>
		/// </summary>
		PushAddress = 0x2d,
		/// <summary>
		/// Calls a Richard function.
		/// <para>call [int func_location]</para>
		/// </summary>
		Call,
	}
}