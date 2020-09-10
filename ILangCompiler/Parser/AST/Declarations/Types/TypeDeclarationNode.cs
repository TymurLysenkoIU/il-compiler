using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Declarations.Types
{
    public class TypeDeclarationNode: ISimpleDeclarationNode
    {
        private TypeDeclarationNode()
        {
            
        }

        private static ParseException NotATypeDeclarationException => new ParseException("Not a type declaration");

        public static Either<ParseException, TypeDeclarationNode> Parse(List<IToken> tokens)
        {
            if (tokens.Count < 3)
                return NotATypeDeclarationException;
            if (!(tokens[0] is TypeKeywordToken) ||
                !(tokens[1] is IdentifierToken) ||
                !(tokens[2] is IsKeywordToken))
                return NotATypeDeclarationException;
            tokens.Skip(3).ToList();

            var maybeType = TypeNode.Parse(tokens);
            if (maybeType.IsLeft)
                return maybeType.LeftToList()[0];
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                    tokens[0] is SemicolonSymbolToken)
                    tokens.Skip(1).ToList();
                else break;
            
            return new TypeDeclarationNode();
        }

    }
}