﻿using System.Collections.Generic;

namespace Rant.Internal.Engine.Metadata
{
    /// <summary>
    /// Provides access to metadata for a Rant function parameter.
    /// </summary>
    public interface IRantParameter
    {
        /// <summary>
        /// Gets the data type accepted by the parameter.
        /// </summary>
        RantParameterType RantType { get; } 

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Indicates whether the parameter accepts multiple values.
        /// </summary>
        bool IsParams { get; }

        /// <summary>
        /// Gets the description for the parameter.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Enumerates all possible values for flag and mode parameters.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IRantModeValue> GetEnumValues();
    }
}