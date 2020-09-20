using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.AST.Expressions;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;
using LanguageExt.ClassInstances.Pred;
using LanguageExt.TypeClasses;

namespace ILangCompiler.Parser.AST.Statements
{
    public class RangeNode:IAstNode
    {
        public ExpressionNode Expression1, Expression2;
        public Boolean Reverse;
        private RangeNode(ExpressionNode expression1, ExpressionNode expression2, Boolean reverse)
        {
            Expression1 = expression1;
            Expression2 = expression2;
            Reverse = reverse;
        }
        private static ParseException NotARangeException => new ParseException("Not a range");

        public static Either<ParseException, Pair<List<IToken>,RangeNode>> Parse(List<IToken> tokens)
        {
            Console.WriteLine("RangeNode");
            if (tokens.Count < 1)
                return NotARangeException;
            if (!(tokens[0] is InKeywordToken))
                return NotARangeException;
            tokens = tokens.Skip(1).ToList();

            Boolean reverse = false;
            if (tokens.Count >= 1)
            {
                if (tokens[0] is ReverseKeywordToken)
                {
                    reverse = true;
                    tokens = tokens.Skip(1).ToList();
                }
            }

            var maybeExpression1 = ExpressionNode.Parse(tokens);
            if (maybeExpression1.IsLeft)
                return maybeExpression1.LeftToList()[0];
            tokens = maybeExpression1.RightToList()[0].First;
            
            if (tokens.Count < 1)
                return NotARangeException;
            if (!(tokens[0] is RangeSymbolToken))
                return NotARangeException;
            tokens = tokens.Skip(1).ToList();
            
            var maybeExpression2 = ExpressionNode.Parse(tokens);
            if (maybeExpression2.IsLeft)
                return maybeExpression2.LeftToList()[0];
            tokens = maybeExpression2.RightToList()[0].First;
            
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                    tokens = tokens.Skip(1).ToList();
                else break;
            
            return new Pair<List<IToken>, RangeNode>(tokens, new RangeNode(
                maybeExpression1.RightToList()[0].Second,
                maybeExpression2.RightToList()[0].Second,
                reverse));
        }
    }
}