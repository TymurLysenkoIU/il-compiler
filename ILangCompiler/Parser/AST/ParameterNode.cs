using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.AST.Declarations;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.Parser.AST.TypeTable;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST
{
    public class ParameterNode : IAstNode
    {

        public IdentifierToken Name;
        public ITypeNode Type;

        private static ParseException NotAParameterException => new ParseException("Not a parameter");

        private ParameterNode(IdentifierToken name, ITypeNode type)
        {
            Name = name;
            Type = type;
        }

        public static Either<ParseException, Pair<List<IToken>, ParameterNode>> Parse(List<IToken> tokens, SymT symT, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Console.WriteLine("ParameterNode");
            if (tokens.Count < 3)
                return NotAParameterException;

            if (!(tokens[0] is IdentifierToken))
            {
                return NotAParameterException;
            }

            IdentifierToken name = (IdentifierToken) tokens[0];
            tokens = tokens.Skip(1).ToList();

            if (!(tokens[0] is IsKeywordToken))
            {
                return NotAParameterException;
            }

            tokens = tokens.Skip(1).ToList();

            var maybeType = TypeNode.Parse(tokens, symT, parentTypeTable);

            if (maybeType.IsLeft)
            {
                return maybeType.LeftToList()[0];
            }

            ITypeNode type = maybeType.RightToList()[0].Second;
            tokens = maybeType.RightToList()[0].First;

            return new Pair<List<IToken>, ParameterNode>(tokens, new ParameterNode(name, type));
        }
    }
}