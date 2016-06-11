using System;
using System.Collections.Generic;
using Rant.Internal.VM.Instructions;

namespace Rant.Internal.VM.Compiler.Parselets
{
    internal class EscapeParselet : IParselet
    {
        public IEnumerator<IParselet> Parse(TokenReader reader, BytecodeGenerator generator)
        {
            var escapeSequence = reader.ReadLoose(R.EscapeSequence, "Escape Sequence");
            // Trim leading backslash from escape sequence.
            var val = escapeSequence.Value.TrimStart('\\');

            // Get amount from escape sequence, if there is one.
            int amount = 1;
            if(val.Contains(","))
            {
                var parts = val.Split(',');
                amount = int.Parse(parts[0]);
                val = parts[1];
            }
            // Get the character.
            var startChar = val[0];

            switch(startChar)
            {
                // It's a dynamic escape sequence.
                case 'a':
                case 'c':
                case 'C':
                case 'd':
                case 'D':
                case 'w':
                case 'W':
                case 'x':
                case 'X':
                    {
                        var bytes = new List<byte>();
                        bytes.Add((byte)startChar);
                        bytes.AddRange(BitConverter.GetBytes(amount));
                        generator.LatestSegment.AddGeneric(RantOpCode.PrintChars, bytes);
                    }
                    break;
                // It's a static escape sequence.
                case 'N':
                    PrintChar(generator, Environment.NewLine);
                    break;
                case 'n':
                    PrintChar(generator, "\n");
                    break;
                case 'r':
                    PrintChar(generator, "\r");
                    break;
                case 'f':
                    PrintChar(generator, "\f");
                    break;
                case 'v':
                    PrintChar(generator, "\v");
                    break;
                case 'b':
                    PrintChar(generator, "\b");
                    break;
                case 's':
                    PrintChar(generator, " ");
                    break;
                case 'u':
                    PrintChar(generator, ((char)Int16.Parse(val.TrimStart('u'), System.Globalization.NumberStyles.AllowHexSpecifier)).ToString());
                    break;
                default:
                    throw new RantCompilerException(reader.SourceName, escapeSequence, "Invalid escape sequence.");
            }
            yield break;
        }

        private void PrintChar(BytecodeGenerator generator, string c)
        {
            generator.LatestSegment.AddStringReference(RantOpCode.PrintStringConstant, c);
        }
    }
}
