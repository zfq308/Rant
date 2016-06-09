namespace Rant.Internal.VM
{
    /// <summary>
    /// Defines object types used by Rant.
    /// </summary>
    public enum RantObjectType
    {
        /// <summary>
        /// Represents a decimal number.
        /// </summary>
        Number,
        /// <summary>
        /// Represents a series of Unicode characters.
        /// </summary>
        String,
        /// <summary>
        /// Represents a boolean value.
        /// </summary>
        Boolean,
        /// <summary>
        /// Represents a resizable set of values.
        /// </summary>
        List,
		/// <summary>
		/// Represents a pointer to an instruction.
		/// </summary>
		Pointer,
        /// <summary>
        /// Represents a lack of a value.
        /// </summary>
        Null,
        /// <summary>
        /// Represents a lack of any variable at all.
        /// </summary>
        Undefined
    }
}