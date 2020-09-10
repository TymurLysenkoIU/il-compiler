using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.AST.Declarations;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
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

        public static Either<ParseException, ParameterNode> Parse(List<IToken> tokens)
        {
            Console.WriteLine("ParameterNode");
            if (tokens.Count < 3)
                return NotAParameterException;

            var maybeIdentifier = tokens[0];
            var maybeColon = tokens[1];

            switch ((tokens[0], tokens[1]))
            {
                case (IdentifierToken it, ColonSymbolToken _):
                    var maybeType = ITypeNode.Parse(tokens.Skip(2).ToList());

                    return maybeType.Map(t => new ParameterNode(it, t));

                    break;
                default:
                    return NotAParameterException;
            }
        }
    }
}