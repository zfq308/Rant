using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rant.Internal.VM.Compiler.Parselets
{
    internal interface IParselet
    {
        IEnumerator<IParselet> Parse(TokenReader reader, BytecodeGenerator generator);
    }
}
