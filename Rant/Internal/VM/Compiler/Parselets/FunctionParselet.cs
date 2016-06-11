using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rant.Internal.VM.Instructions;

namespace Rant.Internal.VM.Compiler.Parselets
{
    internal class FunctionParselet : IParselet
    {
        public IEnumerator<IParselet> Parse(TokenReader reader, BytecodeGenerator generator)
        {
            var name = reader.Read(R.Text, "Function name").Value;
            // read arguments
            while(!reader.End)
            {
                generator.LatestSegment.AddGeneric(RantOpCode.NewOutput)
                
            }

            switch(name)
            {
            }
            yield break;
        }
    }
}
