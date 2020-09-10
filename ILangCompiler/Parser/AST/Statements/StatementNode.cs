using System;
using System.Collections.Generic;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Statements
{
    public class StatementNode:IAstNode
    {
        private StatementNode()
        {
            
        }
        private static ParseException NotAStatementException => new ParseException("Not a statement");

        public static Either<ParseException, Pair<List<IToken>,StatementNode>> Parse(List<IToken> tokens)
        {
            Console.WriteLine("StatementNode");
            var maybeAssignment = AssignmentNode.Parse(tokens);
            if (maybeAssignment.IsRight)
            {
                tokens = maybeAssignment.RightToList()[0].First;
                return new Pair<List<IToken>,StatementNode>(tokens,new StatementNode());
            }

            var maybeRoutineCall = RoutineCallNode.Parse(tokens);
            if (maybeRoutineCall.IsRight)
            {
                tokens = maybeRoutineCall.RightToList()[0].First;
                return new Pair<List<IToken>,StatementNode>(tokens,new StatementNode());
            }

            var maybeWhileLoop = WhileLoopNode.Parse(tokens);
            if (maybeWhileLoop.IsRight)
            {
                tokens = maybeWhileLoop.RightToList()[0].First;
                return new Pair<List<IToken>,StatementNode>(tokens,new StatementNode());
            }

            var maybeForLoop = ForLoopNode.Parse(tokens);
            if (maybeForLoop.IsRight)
            {
                tokens = maybeForLoop.RightToList()[0].First;
                return new Pair<List<IToken>,StatementNode>(tokens,new StatementNode());
            }
                
            var maybeIfStatement = IfStatementNode.Parse(tokens);
            if (maybeIfStatement.IsRight)
            {
                tokens = maybeIfStatement.RightToList()[0].First;
                return new Pair<List<IToken>, StatementNode>(tokens, new StatementNode());
            }

            return NotAStatementException;
        }
    }
}