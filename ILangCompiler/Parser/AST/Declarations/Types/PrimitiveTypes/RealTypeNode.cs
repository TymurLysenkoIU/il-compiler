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
    public class RealTypeNode : IPrimitiveTypeNode
    {
        private RealTypeNode()
        {
            
        }
        private static ParseException NotARealTypeException => new ParseException("Not a real type");

        public static Either<ParseException, RealTypeNode> Parse(List<IToken> tokens)
        {
            Console.WriteLine("RealTypeNode");
            if (tokens.Count < 1)
                return NotARealTypeException;
            if (!(tokens[0] is RealKeywordToken))
                return NotARealTypeException;
            tokens = tokens.Skip(1).ToList();
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken|| 
                    tokens[0] is SemicolonSymbolToken)
                    tokens.Skip(1).ToList();
                else break;
            return new RealTypeNode();
        }
    }
}