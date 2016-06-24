using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Rant.Internal.VM.Compiler.Parselets
{
    /// <summary>
    /// Parses tags.
    /// </summary>
    internal class TagParselet : IParselet
    {
        public IEnumerator<IParselet> Parse(TokenReader reader, BytecodeGenerator generator)
        {
            reader.ReadLoose(R.LeftSquare, "Tag start");
            var nextToken = reader.PeekToken();
            switch(nextToken.ID)
            {
                // function
                case R.Text:
                    yield return new FunctionParselet();
                    break;
                // subrountine
                case R.Dollar:
                    throw new NotImplementedException("Sorry, subroutines aren't implemented yet :(");
                // replacer
                case R.Regex:
                    throw new NotImplementedException("Sorry, replacers aren't implemented yet :(");
                // script
                case R.At:
                    throw new NotImplementedException("Sorry, scripts aren't implemented yet :(");
            }
            yield break;
        }
    }
}
