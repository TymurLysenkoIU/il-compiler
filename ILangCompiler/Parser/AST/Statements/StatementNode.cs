using System;
using System.Collections.Generic;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Statements
{
    public class StatementNode:IAstNode
    {
        public AssignmentNode Assignment;
        public RoutineCallNode RoutineCall;
        public WhileLoopNode WhileLoop;
        public ForLoopNode ForLoop;
        public IfStatementNode IfStatement;
        private StatementNode(AssignmentNode assignment)
        {
            Assignment = assignment;
        }
        private StatementNode(RoutineCallNode routineCall)
        {
            RoutineCall = routineCall;
        }
        private StatementNode(WhileLoopNode whileLoop)
        {
            WhileLoop = whileLoop;
        }
        private StatementNode(ForLoopNode forLoop)
        {
            ForLoop = forLoop;
        }
        private StatementNode(IfStatementNode ifStatement)
        {
            IfStatement = ifStatement;
        }
        private static ParseException NotAStatementException => new ParseException("Not a statement");

        public static Either<ParseException, Pair<List<IToken>,StatementNode>> Parse(List<IToken> tokens)
        {
            Console.WriteLine("StatementNode");
            var maybeAssignment = AssignmentNode.Parse(tokens);
            if (maybeAssignment.IsRight)
            {
                tokens = maybeAssignment.RightToList()[0].First;
                return new Pair<List<IToken>,StatementNode>(tokens,new StatementNode(
                    maybeAssignment.RightToList()[0].Second));
            }

            var maybeRoutineCall = RoutineCallNode.Parse(tokens);
            if (maybeRoutineCall.IsRight)
            {
                tokens = maybeRoutineCall.RightToList()[0].First;
                return new Pair<List<IToken>,StatementNode>(tokens,new StatementNode(
                    maybeRoutineCall.RightToList()[0].Second));
            }

            var maybeWhileLoop = WhileLoopNode.Parse(tokens);
            if (maybeWhileLoop.IsRight)
            {
                tokens = maybeWhileLoop.RightToList()[0].First;
                return new Pair<List<IToken>,StatementNode>(tokens,new StatementNode(
                    maybeWhileLoop.RightToList()[0].Second));
            }

            var maybeForLoop = ForLoopNode.Parse(tokens);
            if (maybeForLoop.IsRight)
            {
                tokens = maybeForLoop.RightToList()[0].First;
                return new Pair<List<IToken>,StatementNode>(tokens,new StatementNode(
                    maybeForLoop.RightToList()[0].Second));
            }
                
            var maybeIfStatement = IfStatementNode.Parse(tokens);
            if (maybeIfStatement.IsRight)
            {
                tokens = maybeIfStatement.RightToList()[0].First;
                return new Pair<List<IToken>, StatementNode>(tokens, new StatementNode(
                    maybeIfStatement.RightToList()[0].Second));
            }

            return NotAStatementException;
        }
    }
}