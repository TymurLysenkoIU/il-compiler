using System.Collections.Generic;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Declarations.Types
{
    public interface ITypeNode : IAstNode
    {
        public static Either<ParseException, ITypeNode> Parse(List<IToken> tokens)
        {
            return new ParseException("Type parsing is not implemented");
        }
    }
}