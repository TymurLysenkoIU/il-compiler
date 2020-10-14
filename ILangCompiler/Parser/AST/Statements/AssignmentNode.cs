using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.AST.Expressions;
using ILangCompiler.Parser.AST.TypeTable;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Operators;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Statements
{
    public class AssignmentNode:IAstNode
    {
        public ModifiablePrimaryNode ModifiablePrimary;
        public ExpressionNode Expression;
        private AssignmentNode(ModifiablePrimaryNode modPrimary, ExpressionNode expression)
        {
            ModifiablePrimary = modPrimary;
            Expression = expression;
        }
        private static ParseException NotAnAssignmentNodeException => new ParseException("Not an assignment");

        public static Either<ParseException, Pair<List<IToken>, AssignmentNode>> Parse(List<IToken> tokens, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Console.WriteLine("AssignmentNode");
            var maybeModifiablePrimary = ModifiablePrimaryNode.Parse(tokens, parentTypeTable);
            if (maybeModifiablePrimary.IsLeft)
                return maybeModifiablePrimary.LeftToList()[0];
            tokens = maybeModifiablePrimary.RightToList()[0].First;

            if (tokens.Count < 1)
                return NotAnAssignmentNodeException;

            if (!(tokens[0] is AssignmentOperatorToken))
                return NotAnAssignmentNodeException;
            tokens = tokens.Skip(1).ToList();

            var maybeExpression = ExpressionNode.Parse(tokens, parentTypeTable);
            if (maybeExpression.IsLeft)
                return maybeExpression.LeftToList()[0];
            tokens = maybeExpression.RightToList()[0].First;

            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                    tokens[0] is SemicolonSymbolToken)
                    tokens = tokens.Skip(1).ToList();
                else break;

            return new Pair<List<IToken>, AssignmentNode>(tokens, new AssignmentNode(
                maybeModifiablePrimary.RightToList()[0].Second, maybeExpression.RightToList()[0].Second));
        }
    }
}