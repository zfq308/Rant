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

		private readonly HashSet<CallFrame> _frameCache = new HashSet<CallFrame>(); 

		private readonly RantProgram _program;
		private readonly long _timeout = 0;
		private readonly Stack<CallFrame> _callStack = new Stack<CallFrame>();
		private readonly Stack<RantObject> _stack = new Stack<RantObject>(4);
		private readonly Stack<RantObject[]> _locals = new Stack<RantObject[]>(); 
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
			int cmp = 0;
			bool cmpValid = false;

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
									Print(_stack.Pop());
									break;
								case RantOpCode.Return:
									_callStack.Pop();
									break;
								case RantOpCode.PushString:
									_stack.Push(new RantObject(_program.Data.GetString(*((int*)&ptrCodeBytes[frame.Position]))));
									frame.Position += sizeof(int);
									break;
								case RantOpCode.PushNum:
									_stack.Push(new RantObject(*((double*)&ptrCodeBytes[frame.Position])));
									frame.Position += sizeof(double);
									break;
								case RantOpCode.Concat:
									rob = _stack.Pop();
									roa = _stack.Pop();
									_stack.Push(new RantObject(String.Concat(roa, rob)));
									break;
								case RantOpCode.Add:
									rob = _stack.Pop();
									roa = _stack.Pop();
									_stack.Push(roa + rob);
									break;
								case RantOpCode.Subtract:
									rob = _stack.Pop();
									roa = _stack.Pop();
									_stack.Push(roa - rob);
									break;
								case RantOpCode.Multiply:
									rob = _stack.Pop();
									roa = _stack.Pop();
									_stack.Push(roa * rob);
									break;
								case RantOpCode.Divide:
									rob = _stack.Pop();
									roa = _stack.Pop();
									_stack.Push(roa / rob);
									break;
								case RantOpCode.Modulo:
									rob = _stack.Pop();
									roa = _stack.Pop();
									_stack.Push(roa % rob);
									break;
								case RantOpCode.Jump:
									frame.Position = *((int*)&ptrCodeBytes[frame.Position]);
									continue;
								case RantOpCode.Compare:
									rob = _stack.Pop();
									roa = _stack.Pop();
									roa.Compare(rob, out cmp, out cmpValid);
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