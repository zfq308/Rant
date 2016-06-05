using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Rant.Internal.IO;
using Rant.Internal.VM.Compiler;

namespace Rant.Internal.VM.Instructions
{
	internal abstract class Op
	{
		private static readonly Dictionary<byte, Type> opCodeMap = new Dictionary<byte, Type>();
		private static readonly Dictionary<Type, byte> opTypeMap = new Dictionary<Type, byte>();
		private readonly byte code;

		static Op()
		{
			Type conflictOp;
			foreach (var opType in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(Op))))
			{
				var attr = opType.GetCustomAttributes(typeof(OpCodeAttribute), false).OfType<OpCodeAttribute>().FirstOrDefault();
				
				if (attr == null)
				{
#if DEBUG
					Console.WriteLine($"WARNING: Op type {opType.Name} has no OpCodeAttribute. Skipping.");
#endif
					continue;
				}
				if (opCodeMap.TryGetValue(attr.Code, out conflictOp))
				{
#if DEBUG
					Console.WriteLine($"WARNING: Opcode assigned to {opType.Name} conflicts with {conflictOp.Name} (OpCode: 0x{attr.Code:X2}). Skipping.");
#endif
					continue;
				}

				opCodeMap[attr.Code] = opType;
				opTypeMap[opType] = attr.Code;
			}
		}

		protected Op()
		{
			if (!opTypeMap.TryGetValue(GetType(), out code))
			{
				throw new RantInternalException($"Op {GetType().Name} does not have an opcode assigned.");
			}
		}

		public static Op Read(EasyReader reader)
		{
			Type opType;
			if (!opCodeMap.TryGetValue(reader.ReadByte(), out opType)) return null;
			var op = Activator.CreateInstance(opType) as Op;
			op.ReadData(reader);
			return op;
		}

		public abstract void ReadData(RantProgramData data, EasyReader reader);

		public abstract void WriteData(RantCompiler compiler, EasyWriter writer);

		public abstract string GetOpString();

		public void WriteOp(RantCompiler compiler, EasyWriter writer)
		{
			writer.Write(code);
			WriteData(compiler, writer);
		}

		public abstract void Run(VM vm);

		public sealed override string ToString() => GetOpString() ?? "<null>";
	}
}