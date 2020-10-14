using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.PrimitiveTypes;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Declarations.Types.PrimitiveTypes
{
    public class BooleanTypeNode : PrimitiveTypeNode, IPrimitiveTypeNode
    {

        private BooleanTypeNode()
        {

        }
        private static ParseException NotABooleanTypeException => new ParseException("Not an boolean type");

        public static Either<ParseException, Pair<List<IToken>,BooleanTypeNode>> Parse(List<IToken> tokens)
        {
            Console.WriteLine("BooleanTypeNode");
            if (tokens.Count < 1)
                return NotABooleanTypeException;
            if (!(tokens[0] is BooleanKeywordToken))
                return NotABooleanTypeException;
            tokens = tokens.Skip(1).ToList();
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken||
                    tokens[0] is SemicolonSymbolToken)
                    tokens = tokens.Skip(1).ToList();
                else break;

            return new Pair <List<IToken>, BooleanTypeNode>(tokens, new BooleanTypeNode());
        }
    }
}