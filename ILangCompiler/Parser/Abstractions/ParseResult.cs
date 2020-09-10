using System.Collections.Immutable;
using ILangCompiler.Scanner.Tokens;

namespace ILangCompiler.Parser.Abstractions
{
    public class ParseResult<TR>
    {
        public TR Result { get; }
        public ImmutableList<IToken> RestTokens { get; }

        public ParseResult(TR result, ImmutableList<IToken> restTokens)
        {
            Result = result;
            RestTokens = restTokens;
        }
    }
}