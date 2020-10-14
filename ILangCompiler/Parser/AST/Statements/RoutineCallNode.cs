using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ILangCompiler.Parser.AST.Expressions;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Statements
{
    public class RoutineCallNode:IAstNode
    {
        public IdentifierToken Identifier;
        public ImmutableArray<ExpressionNode> Expressions;
        private RoutineCallNode(ImmutableArray<ExpressionNode> expressions)
        {
            Expressions = expressions;
        }
        private static ParseException NotARoutineCallException => new ParseException("Not a routine call");

        public static Either<ParseException, Pair<List<IToken>,RoutineCallNode>> Parse(List<IToken> tokens, SymT symT)
        {
            ImmutableArray<ExpressionNode> expressions = ImmutableArray<ExpressionNode>.Empty;
            Console.WriteLine("RoutineCallNode");
            // Identifier [ ( Expression { , Expression } ) ]
            if (tokens.Count < 1)
                return NotARoutineCallException;
            if (!(tokens[0] is IdentifierToken))
                return NotARoutineCallException;
            IdentifierToken identifier = (IdentifierToken) tokens[0];
            tokens = tokens.Skip(1).ToList();
            
          
            
            if (tokens.Count < 1)
                return NotARoutineCallException;
            if (!(tokens[0] is LeftParenthSymbolToken))
                return NotARoutineCallException;
            tokens = tokens.Skip(1).ToList();

            
            var maybeExpression1 = ExpressionNode.Parse(tokens);
            if (maybeExpression1.IsRight)
            {
                expressions = expressions.Add(maybeExpression1.RightToList()[0].Second);
                tokens = maybeExpression1.RightToList()[0].First;
            }

            
            
            while (true)
            {
                if (tokens.Count < 1)
                    break;
                if (!(tokens[0] is ComaSymbolToken))
                    break;
                tokens = tokens.Skip(1).ToList();
                
                var maybeExpression2 = ExpressionNode.Parse(tokens);
                if (maybeExpression2.IsLeft)
                    return NotARoutineCallException;
                expressions = expressions.Add(maybeExpression2.RightToList()[0].Second);
                tokens = maybeExpression2.RightToList()[0].First;
            }


            if (tokens.Count < 1)
                return NotARoutineCallException;
            if (!(tokens[0] is RightParenthSymbolToken))
                return NotARoutineCallException;
            tokens = tokens.Skip(1).ToList();

            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                    tokens = tokens.Skip(1).ToList();
                else break;

            if (!(symT.ContainRec(identifier)))
            {
                Console.Write("Such routine does not exist");
            }

            return new Pair<List<IToken>,RoutineCallNode> (tokens, new RoutineCallNode(
                expressions));
        }
    }
}