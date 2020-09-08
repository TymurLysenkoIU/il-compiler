using System.Collections.Generic;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Declarations
{
    public interface ISimpleDeclarationNode : IDeclarationNode
    {
        public static Either<ParseException, ISimpleDeclarationNode> Parse(List<IToken> tokens)
        {
            
            return new ParseException("Simple declaration is not implemented");
        }
    }
}