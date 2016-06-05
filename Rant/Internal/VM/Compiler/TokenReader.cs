﻿using System;
using System.Collections.Generic;
using System.Linq;

using Rant.Internal.Stringes;

namespace Rant.Internal.VM.Compiler
{
	internal class TokenReader
	{
		private readonly string _sourceName;
		private readonly Token<R>[] _tokens;
		private int _pos;

		public TokenReader(string sourceName, IEnumerable<Token<R>> tokens)
		{
			_sourceName = sourceName;
			_tokens = tokens.ToArray();
			_pos = 0;
		}

		public string SourceName => _sourceName;

		public int Position
		{
			get { return _pos; }
			set { _pos = value; }
		}

		/// <summary>
		/// Determines whether the reader has reached the end of the token stream.
		/// </summary>
		public bool End => _pos >= _tokens.Length;

		/// <summary>
		/// The last token that was read.
		/// </summary>
		public Token<R> PrevToken => _pos == 0 ? null : _tokens[_pos - 1];

		/// <summary>
		/// The last non-whitespace token before the current reader position.
		/// </summary>
		public Token<R> PrevLooseToken
		{
			get
			{
				if (_pos == 0) return null;
				int tempPos = _pos - 1;
				while (tempPos > 0 && _tokens[tempPos].ID == R.Whitespace)
					tempPos--;
				return _tokens[tempPos].ID != R.Whitespace ? _tokens[tempPos] : null;
			}
		}

		/// <summary>
		/// Reads the next available token.
		/// </summary>
		/// <returns></returns>
		public Token<R> ReadToken()
		{
			if (End) throw new RantCompilerException(_sourceName, null, "Unexpected end of file.");
			return _tokens[_pos++];
		}

		/// <summary>
		/// Returns the next available token, but does not consume it.
		/// </summary>
		/// <returns></returns>
		public Token<R> PeekToken()
		{
			return End ? null : _tokens[_pos];
		}

		/// <summary>
		/// Returns the next available non-whitespace token, but does not consume it.
		/// </summary>
		/// <returns></returns>
		public Token<R> PeekLooseToken()
		{
			if (End) throw new RantCompilerException(_sourceName, null, "Unexpected end of file.");
			int pos = _pos;
			SkipSpace();
			var token = _tokens[_pos];
			_pos = pos;
			return token;
		}

		/// <summary>
		/// Returns the type of the next available token.
		/// </summary>
		/// <returns></returns>
		public R PeekType() => End ? R.EOF : _tokens[_pos].ID;

		/// <summary>
		/// Determines whether the next token is of the specified type.
		/// </summary>
		/// <param name="type">The type to check for.</param>
		/// <returns></returns>
		public bool IsNext(R type)
		{
			return !End && _tokens[_pos].ID == type;
		}

		/// <summary>
		/// The last non-whitespace token type.
		/// </summary>
		public R LastNonSpaceType
		{
			get
			{
				if (_pos == 0) return R.EOF;
				R id;
				for (int i = _pos; i >= 0; i--)
				{
					if ((id = _tokens[i].ID) != R.Whitespace) return id;
				}
				return R.EOF;
			}
		}

		/// <summary>
		/// Consumes the next token if its type matches the specified type. Returns true if it matches.
		/// </summary>
		/// <param name="type">The type to consume.</param>
		/// <param name="allowEof">Allow end-of-file tokens. Specifying False will throw an exception instead.</param>
		/// <returns></returns>
		public bool Take(R type, bool allowEof = true)
		{
			if (End)
			{
				if (!allowEof)
					throw new RantCompilerException(_sourceName, null, "Unexpected end-of-file.");
				return false;
			}
			if (_tokens[_pos].ID != type) return false;
			_pos++;
			return true;
		}

		/// <summary>
		/// Consumes the next non-whitespace token if its type matches the specified type. Returns true if it matches.
		/// </summary>
		/// <param name="type">The type to consume.</param>
		/// <param name="allowEof">Allow end-of-file tokens. Specifying False will throw an exception instead.</param>
		/// <returns></returns>
		public bool TakeLoose(R type, bool allowEof = true)
		{
			if (End)
			{
				if (!allowEof)
					throw new RantCompilerException(_sourceName, null, "Unexpected end-of-file.");
				return false;
			}
			SkipSpace();
			if (_tokens[_pos].ID != type) return false;
			_pos++;
			SkipSpace();
			return true;
		}

		/// <summary>
		/// Consumes the next token if its type matches any of the specified types. Returns true if a match was found.
		/// </summary>
		/// <param name="types">The types to consume.</param>
		/// <returns></returns>
		public bool TakeAny(params R[] types)
		{
			if (End) return false;
			foreach (var type in types)
			{
				if (_tokens[_pos].ID != type) continue;
				_pos++;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Consumes the next token if its type matches any of the specified types, and outputs the matching type. Returns true if a match was found.
		/// </summary>
		/// <param name="result">The matched type.</param>
		/// <param name="types">The types to consume.</param>
		/// <returns></returns>
		public bool TakeAny(out R result, params R[] types)
		{
			result = default(R);
			if (End) return false;
			foreach (var type in types)
			{
				if (_tokens[_pos].ID != type) continue;
				result = type;
				_pos++;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Consumes the next non-whitespace token if its type matches any of the specified types. Returns true if a match was found.
		/// </summary>
		/// <param name="types">The types to consume.</param>
		/// <returns></returns>
		public bool TakeAnyLoose(params R[] types)
		{
			if (End) return false;
			SkipSpace();
			foreach (var type in types)
			{
				if (_tokens[_pos].ID != type) continue;
				_pos++;
				SkipSpace();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Consumes the next non-whitespace token if its type matches any of the specified types, and outputs the matching type. Returns true if a match was found.
		/// </summary>
		/// <param name="result">The matched type.</param>
		/// <param name="types">The types to consume.</param>
		/// <returns></returns>
		public bool TakeAnyLoose(out R result, params R[] types)
		{
			result = default(R);
			if (End) return false;
			SkipSpace();
			foreach (var type in types)
			{
				if (_tokens[_pos].ID != type) continue;
				result = type;
				_pos++;
				SkipSpace();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Consumes as many tokens as possible, as long as they match the specified type. Returns true if at least one was found.
		/// </summary>
		/// <param name="type">The type to consume.</param>
		/// <param name="allowEof">Allow end-of-file tokens. Specifying False will throw an exception instead.</param>
		/// <returns></returns>
		public bool TakeAll(R type, bool allowEof = true)
		{
			if (End)
			{
				if (!allowEof)
					throw new RantCompilerException(_sourceName, null, "Unexpected end-of-file.");
				return false;
			}
			if (_tokens[_pos].ID != type) return false;
			do
			{
				_pos++;
			} while (!End && _tokens[_pos].ID == type);
			return true;
		}

		/// <summary>
		/// Consumes as many non-whitespace tokens as possible, as long as they match the specified type. Returns true if at least one was found.
		/// </summary>
		/// <param name="type">The type to consume.</param>
		/// <param name="allowEof">Allow end-of-file tokens. Specifying False will throw an exception instead.</param>
		/// <returns></returns>
		public bool TakeAllLoose(R type, bool allowEof = true)
		{
			if (End)
			{
				if (!allowEof)
					throw new RantCompilerException(_sourceName, null, "Unexpected end-of-file.");
				return false;
			}
			SkipSpace();
			if (_tokens[_pos].ID != type) return false;
			do
			{
				SkipSpace();
				_pos++;
			} while (!End && _tokens[_pos].ID == type);
			return true;
		}

		/// <summary>
		/// Reads and returns the next token if its type matches the specified type.
		/// If it does not match, a RantCompilerException is thrown with the expected token name.
		/// </summary>
		/// <param name="type">The token type to read.</param>
		/// <param name="expectedTokenName">A display name describing what the token is for.</param>
		/// <returns></returns>
		public Token<R> Read(R type, string expectedTokenName = null)
		{
			if (End)
				throw new RantCompilerException(_sourceName, null, "Expected " + (expectedTokenName ?? "'" + RantLexer.Rules.GetSymbolForId(type) + "'") + ", but hit end of file.");
			if (_tokens[_pos].ID != type)
			{
				throw new RantCompilerException(_sourceName, _tokens[_pos], "Expected " + (expectedTokenName ?? "'" + RantLexer.Rules.GetSymbolForId(type) + "'"));
			}
			return _tokens[_pos++];
		}

		/// <summary>
		/// Reads and returns the next token if its type matches any of the given types
		/// If it does not match, a RantCompilerException is thrown with the expected token names.
		/// </summary>
		/// <param name="types">The token types accepted for the read token.</param>
		/// <returns></returns>
		public Token<R> ReadAny(params R[] types)
		{
			if (End)
				throw new RantCompilerException(_sourceName, null,
					$"Expected any from {{{String.Join(", ", types.Select(t => RantLexer.Rules.GetSymbolForId(t)).ToArray())}}}, but hit end of file.");

			if (!types.Contains(_tokens[_pos].ID)) // NOTE: .Contains isn't too fast but does it matter in this case?
				throw new RantCompilerException(_sourceName, _tokens[_pos],
					$"Expected any from {{{String.Join(", ", types.Select(t => RantLexer.Rules.GetSymbolForId(t)).ToArray())}}}.");

			return _tokens[_pos++];
		}

		/// <summary>
		/// Reads and returns the next non-whitespace token if its type matches the specified type.
		/// If it does not match, a RantCompilerException is thrown with the expected token name.
		/// </summary>
		/// <param name="type">The token type to read.</param>
		/// <param name="expectedTokenName">A display name describing what the token is for.</param>
		/// <returns></returns>
		public Token<R> ReadLoose(R type, string expectedTokenName = null)
		{
			if (End)
				throw new RantCompilerException(_sourceName, null, "Expected " + (expectedTokenName ?? "'" + RantLexer.Rules.GetSymbolForId(type) + "'") + ", but hit end of file.");
			SkipSpace();
			if (_tokens[_pos].ID != type)
			{
				throw new RantCompilerException(_sourceName, _tokens[_pos], "Expected " + (expectedTokenName ?? "'" + RantLexer.Rules.GetSymbolForId(type) + "'"));
			}
			var t = _tokens[_pos++];
			SkipSpace();
			return t;
		}

		/// <summary>
		/// Reads a series of tokens into a buffer as long as they match the types specified in an array, in the order they appear. Returns True if reading was successful.
		/// </summary>
		/// <param name="buffer">The buffer to read into.</param>
		/// <param name="offset">The offset at which to begin writing tokens into the buffer.</param>
		/// <param name="types">The required types.</param>
		/// <returns></returns>
		public bool TakeSeries(Token<R>[] buffer, int offset, params R[] types)
		{
			if (_pos >= _tokens.Length) return types.Length == 0;
			if (_tokens.Length - _pos < types.Length || buffer.Length - offset < types.Length)
				return false;
			for (int i = 0; i < types.Length; i++)
			{
				if (_tokens[_pos + i].ID != types[i]) return false;
				buffer[i + offset] = _tokens[_pos + i];
			}
			_pos += types.Length;
			return true;
		}

		/// <summary>
		/// Reads a series of non-whitespace tokens into a buffer as long as they match the types specified in an array, in the order they appear. Returns True if reading was successful.
		/// </summary>
		/// <param name="buffer">The buffer to read into.</param>
		/// <param name="offset">The offset at which to begin writing tokens into the buffer.</param>
		/// <param name="types">The required types.</param>
		/// <returns></returns>
		public bool TakeSeriesLoose(Token<R>[] buffer, int offset, params R[] types)
		{
			if (_pos >= _tokens.Length) return types.Length == 0;
			if (_tokens.Length - _pos < types.Length || buffer.Length - offset < types.Length)
				return false;
			int i = 0;
			int j = 0;
			while (i < types.Length)
			{
				if (_pos + j >= _tokens.Length) return false;
				while (_tokens[_pos + j].ID == R.Whitespace) j++;
				if (_pos + j >= _tokens.Length) return false;

				if (_tokens[_pos + j].ID != types[i]) return false;
				buffer[i + offset] = _tokens[_pos + i];
				j++;
				i++;
			}
			_pos += j;
			return true;
		}

		/// <summary>
		/// Reads and returns the next non-whitespace token.
		/// </summary>
		/// <returns></returns>
		public Token<R> ReadLooseToken()
		{
			if (End)
				throw new RantCompilerException(_sourceName, null, "Expected token, but hit end of file.");
			SkipSpace();
			var token = _tokens[_pos++];
			SkipSpace();
			return token;
		}

		/// <summary>
		/// Consumes as many token as possible while they satisfy the specified predicate.
		/// </summary>
		/// <param name="predicate">The predicate to test tokens with.</param>
		/// <param name="allowEof">Allow end-of-file tokens. Specifying False will throw an exception instead.</param>
		/// <returns></returns>
		public bool TakeAllWhile(Func<Token<R>, bool> predicate, bool allowEof = true)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			if (End)
			{
				if (!allowEof)
					throw new RantCompilerException(_sourceName, null, "Unexpected end-of-file.");
				return false;
			}

			int i = _pos;
			Token<R> t;
			while (_pos < _tokens.Length)
			{
				t = _tokens[_pos];
				if (!predicate(t))
				{
					return _pos > i;
				}
				_pos++;
			}
			return true;
		}

		public bool SkipSpace() => TakeAll(R.Whitespace);

		public Token<R> this[int pos] => _tokens[pos];
	}
}