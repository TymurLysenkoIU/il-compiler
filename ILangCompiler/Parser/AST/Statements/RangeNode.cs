﻿using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.AST.Expressions;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Statements
{
    public class RangeNode
    {
        private RangeNode()
        {
            
        }
        private static ParseException NotARangeException => new ParseException("Not a range");

        public static Either<ParseException, RangeNode> Parse(List<IToken> tokens)
        {
            if (tokens.Count < 1)
                return NotARangeException;
            if (!(tokens[0] is InKeywordToken))
                return NotARangeException;
            tokens.Skip(1).ToList();

            if (tokens.Count >= 1)
            {
                if (tokens[0] is ReverseKeywordToken)
                    tokens.Skip(1).ToList();
            }

            var maybeExpression1 = ExpressionNode.Parse(tokens);
            if (maybeExpression1.IsLeft)
                return maybeExpression1.LeftToList()[0];
            
            if (tokens.Count < 1)
                return NotARangeException;
            if (!(tokens[0] is RangeSymbolToken))
                return NotARangeException;
            tokens.Skip(1).ToList();
            
            var maybeExpression2 = ExpressionNode.Parse(tokens);
            if (maybeExpression2.IsLeft)
                return maybeExpression2.LeftToList()[0];
            
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                    tokens.Skip(1).ToList();
                else break;
            
            return new RangeNode();
        }
    }
}