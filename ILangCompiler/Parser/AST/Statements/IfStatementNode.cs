using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.AST.Expressions;
using ILangCompiler.Parser.AST.TypeTable;
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
        public ExpressionNode Expression;
        public BodyNode Body1;
        public Option<BodyNode> Body2;
        private IfStatementNode(ExpressionNode expression, BodyNode body1, Option<BodyNode> body2)
        {
            Expression = expression;
            Body1 = body1;
            Body2 = body2;
        }
        private static ParseException NotAnIfStatementException => new ParseException("Not an if statement");

        public static Either<ParseException, Pair<List<IToken>,IfStatementNode>> Parse(List<IToken> tokens, SymT symT, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Console.WriteLine("IfStatementNode");
            if (tokens.Count < 1)
                return NotAnIfStatementException;
            if (!(tokens[0] is IfKeywordToken))
                return NotAnIfStatementException;
            tokens = tokens.Skip(1).ToList();

            var maybeExpression = ExpressionNode.Parse(tokens, parentTypeTable);
            if (maybeExpression.IsLeft)
                return NotAnIfStatementException;
            tokens = maybeExpression.RightToList()[0].First;

            if (tokens.Count < 1)
                return NotAnIfStatementException;
            if (!(tokens[0] is ThenKeywordToken))
                return NotAnIfStatementException;
            tokens = tokens.Skip(1).ToList();

            var maybeBody1 = BodyNode.Parse(tokens, symT, parentTypeTable);
            if (maybeBody1.IsLeft)
                return NotAnIfStatementException;
            tokens = maybeBody1.RightToList()[0].First;

            if (tokens.Count < 1)
                return NotAnIfStatementException;
            Either<ParseException, Pair<List<IToken>, BodyNode>> maybeBody2 = new ParseException("Dummy");
            if (tokens[0] is ElseKeywordToken)
            {
                tokens = tokens.Skip(1).ToList();

                maybeBody2 = BodyNode.Parse(tokens, symT, parentTypeTable);

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

            return new Pair<List<IToken>,IfStatementNode> (tokens, new IfStatementNode(
                maybeExpression.RightToList()[0].Second, maybeBody1.RightToList()[0].Second,
                maybeBody2.Map<BodyNode>(pr => pr.Second).ToOption()));
        }
    }
}