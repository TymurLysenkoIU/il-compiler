using System.Collections.Generic;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Statements
{
    public class StatementNode
    {
        private StatementNode()
        {
            
        }
        private static ParseException NotAStatementException => new ParseException("Not a statement");

        public static Either<ParseException, StatementNode> Parse(List<IToken> tokens)
        {
            var maybeAssignment = AssignmentNode.Parse(tokens);
            if (maybeAssignment.IsRight)
                return new StatementNode();
            var maybeRoutineCall = RoutineCallNode.Parse(tokens);
            if (maybeRoutineCall.IsRight)
                return new StatementNode();
            var maybeWhileLoop = WhileLoopNode.Parse(tokens);
            if (maybeWhileLoop.IsRight)
                return new StatementNode();
            var maybeForLoop = ForLoopNode.Parse(tokens);
            if (maybeForLoop.IsRight)
                return new StatementNode();
            var maybeIfStatement = IfStatementNode.Parse(tokens);
            if (maybeIfStatement.IsRight)
                return new StatementNode();

            return NotAStatementException;
        }
    }
}