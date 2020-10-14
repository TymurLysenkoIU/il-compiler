using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Literals;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.PrimitiveTypes;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Declarations.Types.PrimitiveTypes
{
    public class IntegerTypeNode : PrimitiveTypeNode, IPrimitiveTypeNode
    {
        private IntegerTypeNode()
        {

        }
        private static ParseException NotAnIntegerTypeException => new ParseException("Not an integer type");

        public static Either<ParseException, Pair<List<IToken>,IntegerTypeNode>> Parse(List<IToken> tokens)
        {
            Console.WriteLine("IntegerTypeNode");
            if (tokens.Count < 1)
                return NotAnIntegerTypeException;
            if (!(tokens[0] is IntegerKeywordToken))
                return NotAnIntegerTypeException;
            tokens = tokens.Skip(1).ToList();
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken||
                    tokens[0] is SemicolonSymbolToken)
                    tokens = tokens.Skip(1).ToList();
                else break;
            return new Pair <List<IToken>, IntegerTypeNode>(tokens, new IntegerTypeNode());

        }
    }
}