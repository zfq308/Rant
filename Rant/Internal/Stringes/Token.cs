﻿using System;

namespace Rant.Internal.Stringes
{
    /// <summary>
    /// Represents a token with a string value and a custom identifier.
    /// </summary>
    /// <typeparam name="T">The identifier type.</typeparam>
    internal sealed class Token<T> : Stringe where T : struct
    {
        private readonly T _id;

        /// <summary>
        /// The token identifier.
        /// </summary>
        public T ID => _id;

        public Token(T id, string value) : base(value)
        {
            _id = id;
        }

        public Token(T id, Stringe value) : base(value)
        {
            _id = id;
        }

		/// <summary>
		/// Returns a string representation of the current token.
		/// </summary>
		/// <returns></returns>
        public override string ToString() => $"{_id}, L{Line}, C{Column}{(String.IsNullOrEmpty(Value) ? "" : $", {Value} ")}";
    }
}