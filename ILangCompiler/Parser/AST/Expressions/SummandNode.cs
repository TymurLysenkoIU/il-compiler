using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Expressions
{
    public class SummandNode:IAstNode
    {
        public PrimaryNode Primary;
        public ExpressionNode Expression;
        private SummandNode(PrimaryNode primary)
        {
            Primary = primary;
        }
        private SummandNode(ExpressionNode expression)
        {
            Expression = expression;
        }

        private static ParseException NotASummandException => new ParseException("Not a summand");

        public static Either<ParseException, Pair<List<IToken>, SummandNode>> Parse(List<IToken> tokens)
        {
            Console.WriteLine("SummandNode");
            var maybePrimary = PrimaryNode.Parse(tokens);
            if (maybePrimary.IsRight)
            {
                tokens = maybePrimary.RightToList()[0].First;
                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;
                return new Pair<List<IToken>, SummandNode>(tokens, new SummandNode(maybePrimary.RightToList()[0].Second));
            }

            
            if (tokens.Count < 1)
                return NotASummandException;
            if (!(tokens[0] is LeftParenthSymbolToken))
                return NotASummandException;
            tokens = tokens.Skip(1).ToList();


            
            var maybeExpression = ExpressionNode.Parse(tokens);
            if (maybeExpression.IsLeft)
                return maybeExpression.LeftToList()[0];
            tokens = maybeExpression.RightToList()[0].First;
            

            
            if (tokens.Count < 1)
                return NotASummandException;
            if (!(tokens[0] is RightParenthSymbolToken))
                return NotASummandException;
            tokens = tokens.Skip(1).ToList();
            
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                    tokens = tokens.Skip(1).ToList();
                else break;
            
            
            
            return new Pair<List<IToken>, SummandNode>(tokens, new SummandNode(maybeExpression.RightToList()[0].Second));
        }

    }
}