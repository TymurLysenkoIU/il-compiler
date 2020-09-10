using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.AST.Expressions;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.Constructions;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Statements
{
    public class IfStatementNode:IAstNode
    {
        private IfStatementNode()
        {
            
        }
        private static ParseException NotAnIfStatementException => new ParseException("Not an if statement");

        public static Either<ParseException, Pair<List<IToken>,IfStatementNode>> Parse(List<IToken> tokens)
        {
            Console.WriteLine("IfStatementNode");
            if (tokens.Count < 1)
                return NotAnIfStatementException;
            if (!(tokens[0] is IfKeywordToken))
                return NotAnIfStatementException;
            tokens = tokens.Skip(1).ToList();

            var maybeExpression = ExpressionNode.Parse(tokens);
            if (maybeExpression.IsLeft)
                return NotAnIfStatementException;
            tokens = maybeExpression.RightToList()[0].First;
            
            if (tokens.Count < 1)
                return NotAnIfStatementException;
            if (!(tokens[0] is ThenKeywordToken))
                return NotAnIfStatementException;
            tokens = tokens.Skip(1).ToList();
            
            var maybeBody1 = BodyNode.Parse(tokens);
            if (maybeBody1.IsLeft)
                return NotAnIfStatementException;
            tokens = maybeBody1.RightToList()[0].First;
            
            if (tokens.Count < 1)
                return NotAnIfStatementException;
            if (tokens[0] is ElseKeywordToken)
            {
                tokens = tokens.Skip(1).ToList();
                var maybeBody2 = BodyNode.Parse(tokens);
                if (maybeBody2.IsLeft)
                    return NotAnIfStatementException;
                tokens = maybeBody2.RightToList()[0].First;
            }
            
            
            if (tokens.Count < 1)
                return NotAnIfStatementException;
            if (!(tokens[0] is EndKeywordToken))
                return NotAnIfStatementException;
            tokens = tokens.Skip(1).ToList();

            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                    tokens[0] is SemicolonSymbolToken)
                    tokens = tokens.Skip(1).ToList();
                else break;

            return new Pair<List<IToken>,IfStatementNode> (tokens, new IfStatementNode());
        }
    }
}