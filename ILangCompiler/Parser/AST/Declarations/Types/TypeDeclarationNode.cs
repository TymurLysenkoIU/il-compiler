using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;
using LanguageExt.ClassInstances.Pred;

namespace ILangCompiler.Parser.AST.Declarations.Types
{
    public class TypeDeclarationNode: ISimpleDeclarationNode
    {
        public IdentifierToken Identifier;
        public TypeNode Type;
        private TypeDeclarationNode(IdentifierToken identifier, TypeNode type)
        {
            Identifier = identifier;
            Type = type;
        }

        private static ParseException NotATypeDeclarationException => new ParseException("Not a type declaration");
        private static ParseException RepeatedIdentifierException => new ParseException("Repeating identifier in the same scope");
        public static Either<ParseException, Pair<List<IToken>,TypeDeclarationNode>> Parse(List<IToken> tokens, SymT symT)
        {
            Console.WriteLine("TypeDeclarationNode");
            if (tokens.Count < 3)
                return NotATypeDeclarationException;
            if (!(tokens[0] is TypeKeywordToken) ||
                !(tokens[1] is IdentifierToken) ||
                !(tokens[2] is IsKeywordToken))
                return NotATypeDeclarationException;
            
            
            IdentifierToken identifier = (IdentifierToken) tokens[1];
            tokens = tokens.Skip(3).ToList();

            var maybeType = TypeNode.Parse(tokens, symT);
            if (maybeType.IsLeft)
                return maybeType.LeftToList()[0];
            tokens = maybeType.RightToList()[0].First;
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                    tokens[0] is SemicolonSymbolToken)
                    tokens = tokens.Skip(1).ToList();
                else break;
            
            if (symT.Contain(identifier))
            {
                return RepeatedIdentifierException;
            }
            else
            {
                symT.Add(identifier);
            }
            return new Pair<List<IToken>, TypeDeclarationNode>(tokens, new TypeDeclarationNode(
                identifier, maybeType.RightToList()[0].Second));
        }

    }
}