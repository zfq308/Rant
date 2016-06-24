using System;
using System.Collections.Generic;

using Rant.Internal.VM.ObjectModel;

namespace Rant.Internal.VM
{
	internal sealed class CallStack
	{
		private const int CapacityMultiplier = 2;
		private const int InitialCapacity = 8;

		private readonly RVM _vm;
		private RantObject[] _stack = new RantObject[InitialCapacity];
		private readonly Stack<Frame> _frames = new Stack<Frame>();
		private Frame _currentFrame; 
		private int _nextAvailableIndex = 0;

		public int Size => _nextAvailableIndex;

		public CallStack(RVM vm)
		{
			_vm = vm;
			_frames.Push(_currentFrame = new Frame(vm, _vm.Program, 0, 0)); 
		}

		public RantObject Peek() => _nextAvailableIndex == 0 ? null : _stack[_nextAvailableIndex - 1];

		public void Push(RantObject obj)
		{
			if (_nextAvailableIndex >= _stack.Length)
			{
				Array.Resize(ref _stack, _stack.Length * CapacityMultiplier);
			}

			_stack[_nextAvailableIndex++] = obj;
		}

		public RantObject Pop()
		{
			if (_nextAvailableIndex == 0)
				throw new RantInternalException("Tried to pop from an empty stack.");

			return _stack[(--_nextAvailableIndex)];
		}

		public void Call(int address, int argc)
		{
			_frames.Push(_currentFrame = new Frame(_vm, this, argc, address));
		}

		public int Position
		{
			get { return _currentFrame.Position; }
			set { _currentFrame.Position = value; }
		}

		public void Return()
		{
			
		}
	}
}