using System;
using System.Collections.Generic;
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
        private RoutineCallNode()
        {
            
        }
        private static ParseException NotARoutineCallException => new ParseException("Not a routine call");

        public static Either<ParseException, Pair<List<IToken>,RoutineCallNode>> Parse(List<IToken> tokens)
        {
            Console.WriteLine("RoutineCallNode");
            // Identifier [ ( Expression { , Expression } ) ]
            if (tokens.Count < 1)
                return NotARoutineCallException;
            if (!(tokens[0] is IdentifierToken))
                return NotARoutineCallException;
            tokens = tokens.Skip(1).ToList();
            
          
            
            if (tokens.Count < 1)
                return NotARoutineCallException;
            if (!(tokens[0] is LeftParenthSymbolToken))
                return NotARoutineCallException;
            tokens = tokens.Skip(1).ToList();

            
            var maybeExpression1 = ExpressionNode.Parse(tokens);
            if (maybeExpression1.IsRight)
            {
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
            
            return new Pair<List<IToken>,RoutineCallNode> (tokens, new RoutineCallNode());
        }
    }
}