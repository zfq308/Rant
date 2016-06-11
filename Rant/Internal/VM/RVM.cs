using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

using Rant.Formats;
using Rant.Internal.Engine;
using Rant.Internal.VM.Instructions;
using Rant.Internal.VM.Output;

namespace Rant.Internal.VM
{
	internal unsafe class RVM
	{
		public readonly RNG RNG;
		public readonly long StartingGen;
		public readonly Limit SizeLimit;
		public RantFormat Format;

		private readonly RantProgram _program;
		private readonly long _timeout = 0;
		private readonly Stack<CallFrame> _callStack = new Stack<CallFrame>();
		private readonly Stack<RantObject> _locals = new Stack<RantObject>(4);
		private readonly Stack<RantObject> _args = new Stack<RantObject>();
		private readonly Stack<OutputWriter> _writers = new Stack<OutputWriter>();

		private int _dbgLine, _dbgCol, _dbgIndex;

		public void Print(object value)
		{
			_writers.Peek().Print(value);
		}

		public RVM(RantProgram program, long timeoutMilliseconds, RNG rng, RantFormat format, int limit)
		{
			_program = program;
			_timeout = timeoutMilliseconds;
			RNG = rng;
			StartingGen = rng.Generation;
			SizeLimit = new Limit(limit);
			Format = format;
		}

#if !UNITY
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		private void ReadInt32(ref int position, out int output)
		{
			output = BitConverter.ToInt32(_program.Bytecode, position);
			position += sizeof(int);
		}

#if !UNITY
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		private void PrintStringIndex(ref int position)
		{
			Print(_program.Data.GetString(BitConverter.ToInt32(_program.Bytecode, position)));
			position += sizeof(int);
		}

		public void RuntimeError(string message)
		{
			throw new RantRuntimeException(_program, _dbgLine, _dbgCol, _dbgIndex, message);
		}

		public RantOutput Run()
		{
			CallFrame frame;
			RantOpCode* ptrCodeOps;
			byte* ptrCodeBytes;
			RantObject roa, rob;

			try
			{
				while (_callStack.Any())
				{
					fixed (void* ptrBytecodeGeneric = _program.Bytecode)
					{
						ptrCodeOps = (RantOpCode*)ptrBytecodeGeneric;
						ptrCodeBytes = (byte*)ptrBytecodeGeneric;

						frame = _callStack.Peek();

						while (frame.Position < _program.Bytecode.Length)
						{
							switch (ptrCodeOps[frame.Position++])
							{
								case RantOpCode.Debug:
									ReadInt32(ref frame.Position, out _dbgLine);
									ReadInt32(ref frame.Position, out _dbgCol);
									ReadInt32(ref frame.Position, out _dbgIndex);
									break;
								case RantOpCode.PrintStringConstant:
									PrintStringIndex(ref frame.Position);
									break;
								case RantOpCode.PrintNumberConstant:
									Print(*((double*)&ptrCodeBytes[frame.Position]));
									frame.Position += sizeof(double);
									break;
								case RantOpCode.OpenChannelConstant:
									_writers.Peek().OpenChannel(
										_program.Data.GetString(*((int*)&ptrCodeBytes[frame.Position])),
										*((ChannelVisibility*)&ptrCodeBytes[frame.Position + sizeof(int)]));
									frame.Position += sizeof(int) + 1;
									break;
								case RantOpCode.CloseChannelConstant:
									_writers.Peek().CloseChannel();
									break;
								case RantOpCode.Print:
									Print(_locals.Pop());
									break;
								case RantOpCode.Return:
									_callStack.Pop();
									break;
							}
							frame.Position++;
						}
					}
				}
				return _writers.Pop().ToRantOutput();
			}
			catch (RantRuntimeException)
			{
				throw;
			}
			catch (RantInternalException)
			{
				throw;
			}
			catch (IndexOutOfRangeException ex)
			{
				throw new RantInternalException("RANT VM: Unexpectedly reached end of bytecode!", ex);
			}
			catch (Exception ex)
			{
				throw new RantInternalException($"Internal VM exception occurred: {ex.Message}", ex);
			}
		}
	}
}