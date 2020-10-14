using System;
using System.Collections.Generic;
using ILangCompiler.Parser.AST.TypeTable;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ILangCompiler.Parser.AST.Statements
{
    public class StatementNode : IAstNode, ITypeTable<IEntityType>
    {
        public AssignmentNode Assignment;
        public RoutineCallNode RoutineCall;
        public WhileLoopNode WhileLoop;
        public ForLoopNode ForLoop;
        public IfStatementNode IfStatement;

        #region Type table

        private readonly IScopedTable<IEntityType, string> ParentTypeTable;

        IDictionary<string, IEntityType> IScopedTable<IEntityType, string>.Table => new Dictionary<string, IEntityType>();

        Option<IScopedTable<IEntityType, string>> IScopedTable<IEntityType, string>.ParentTable => Some(ParentTypeTable);

        #endregion


        private StatementNode(AssignmentNode assignment, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Assignment = assignment;
            ParentTypeTable = parentTypeTable;
        }
        private StatementNode(RoutineCallNode routineCall, IScopedTable<IEntityType, string> parentTypeTable)
        {
            RoutineCall = routineCall;
            ParentTypeTable = parentTypeTable;
        }
        private StatementNode(WhileLoopNode whileLoop, IScopedTable<IEntityType, string> parentTypeTable)
        {
            WhileLoop = whileLoop;
            ParentTypeTable = parentTypeTable;
        }
        private StatementNode(ForLoopNode forLoop, IScopedTable<IEntityType, string> parentTypeTable)
        {
            ForLoop = forLoop;
            ParentTypeTable = parentTypeTable;
        }
        private StatementNode(IfStatementNode ifStatement, IScopedTable<IEntityType, string> parentTypeTable)
        {
            IfStatement = ifStatement;
            ParentTypeTable = parentTypeTable;
        }
        private static ParseException NotAStatementException => new ParseException("Not a statement");

        public static Either<ParseException, Pair<List<IToken>,StatementNode>> Parse(List<IToken> tokens, SymT symT, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Console.WriteLine("StatementNode");

            var maybeAssignment = AssignmentNode.Parse(tokens, parentTypeTable);
            if (maybeAssignment.IsRight)
            {
                tokens = maybeAssignment.RightToList()[0].First;
                return new Pair<List<IToken>,StatementNode>(tokens, new StatementNode(
                        maybeAssignment.RightToList()[0].Second,
                        parentTypeTable));
            }

            var maybeRoutineCall = RoutineCallNode.Parse(tokens, symT, parentTypeTable);
            if (maybeRoutineCall.IsRight)
            {
                tokens = maybeRoutineCall.RightToList()[0].First;
                return new Pair<List<IToken>,StatementNode>(tokens,new StatementNode(
                    maybeRoutineCall.RightToList()[0].Second,
                    parentTypeTable));
            }

            var maybeWhileLoop = WhileLoopNode.Parse(tokens, symT, parentTypeTable);
            if (maybeWhileLoop.IsRight)
            {
                tokens = maybeWhileLoop.RightToList()[0].First;
                return new Pair<List<IToken>,StatementNode>(tokens,new StatementNode(
                    maybeWhileLoop.RightToList()[0].Second,
                    parentTypeTable));
            }

            var maybeForLoop = ForLoopNode.Parse(tokens, symT, parentTypeTable);
            if (maybeForLoop.IsRight)
            {
                tokens = maybeForLoop.RightToList()[0].First;
                return new Pair<List<IToken>,StatementNode>(tokens,new StatementNode(
                    maybeForLoop.RightToList()[0].Second,
                    parentTypeTable));
            }

            var maybeIfStatement = IfStatementNode.Parse(tokens, symT, parentTypeTable);
            if (maybeIfStatement.IsRight)
            {
                tokens = maybeIfStatement.RightToList()[0].First;
                return new Pair<List<IToken>, StatementNode>(tokens, new StatementNode(
                    maybeIfStatement.RightToList()[0].Second,
                    parentTypeTable));
            }

            return NotAStatementException;
        }
    }
}