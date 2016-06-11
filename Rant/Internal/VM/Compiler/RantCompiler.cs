using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Rant.Internal.VM.Compiler.Parselets;
using Rant.Internal.Engine.Syntax;
using Rant.Internal.Stringes;

namespace Rant.Internal.VM.Compiler
{
    internal class RantCompiler
    {
        // Code, String Table
        public static byte PATTERN_SECTION_COUNT = 2;
        public static byte CODE_SECTION_ID = 0x01;
        public static byte STRING_TABLE_SECTION_ID = 0x02;
        public static string MAGIC_NUMBER = "RPTN";

        public static byte[] Compile(string sourceName, string source) => new RantCompiler(sourceName, source).Compile();

        private readonly string _source;
        private readonly string _sourceName;
        private readonly TokenReader _reader;
        private BytecodeGenerator _generator;

        public RantCompiler(string sourceName, string source)
        {
            _source = source;
            _sourceName = sourceName;

            _reader = new TokenReader(sourceName, RantLexer.GenerateTokens(sourceName, source.ToStringe()));
        }

        public byte[] Compile()
        {
            _generator = new BytecodeGenerator();
            var parseletStack = new Stack<IEnumerator<IParselet>>();
            parseletStack.Push(new PatternParselet().Parse(_reader, _generator));

            top:
            while(parseletStack.Any())
            {
                var currentParselet = parseletStack.Peek();
                while(currentParselet.MoveNext())
                {
                    if(currentParselet.Current == null) break;
                    parseletStack.Push(currentParselet.Current.Parse(_reader, _generator));
                    goto top;
                }
                parseletStack.Pop();
            }

            // Assemble final pattern.
            var bytecode = _generator.Compile();
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            writer.Write(MAGIC_NUMBER.ToCharArray());
            // Section header
            writer.Write(PATTERN_SECTION_COUNT);
            // Write dummy address for sections, for now
            writer.Write(CODE_SECTION_ID);
            var codeAddrPosition = stream.Position;
            writer.Write((int)0);
            writer.Write(STRING_TABLE_SECTION_ID);
            var stringTablePosition = stream.Position;
            writer.Write((int)0);

            // Write code address
            var currentAddr = stream.Position;
            writer.Seek((int)codeAddrPosition, SeekOrigin.Begin);
            writer.Write((int)currentAddr);
            writer.Seek((int)currentAddr, SeekOrigin.Begin);
            // Write code
            writer.Write(bytecode.Length);
            writer.Write(bytecode);

            // Write string table address
            currentAddr = stream.Position;
            writer.Seek((int)stringTablePosition, SeekOrigin.Begin);
            writer.Write((int)currentAddr);
            writer.Seek((int)currentAddr, SeekOrigin.Begin);
            // Write string table
            writer.Write(_generator.BuildStringTable());

            return stream.ToArray();
        }
    }
}
