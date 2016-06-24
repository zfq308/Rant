namespace Rant.Internal.VM
{
	/// <summary>
	/// Represents an assembly-specific bytecode address.
	/// </summary>
	public struct Handle
	{
		/// <summary>
		/// The local assembly handle points to the assembly that the address is found in.
		/// The handle refers to an index in the Imports section of the program's data table
		/// corresponding with the desired assembly. The assembly loader examines this table
		/// and resolves each reference, building a table that maps these handles to the correct assembly.
		/// <para>NOTE: Setting this to -1 indicates that the address is inside the executing assembly.</para>
		/// </summary>
		public int LocalAssemblyHandle;
		/// <summary>
		/// The address inside of the specified assembly.
		/// </summary>
		public int Address;
	}
}