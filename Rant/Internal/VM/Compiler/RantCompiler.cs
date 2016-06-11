using System;
using System.Collections.Generic;
using System.Linq;

using Rant.Internal.VM.Compiler.Parselets;
using Rant.Internal.Engine.Syntax;
using Rant.Internal.Stringes;

namespace Rant.Internal.VM.Compiler
{
    internal class RantCompiler
    {
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

            return _generator.Compile();
        }
    }
}
