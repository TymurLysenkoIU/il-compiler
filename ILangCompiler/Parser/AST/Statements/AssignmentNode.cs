using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.AST.Expressions;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Operators;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Statements
{
    public class AssignmentNode:IAstNode
    {
        private AssignmentNode()
        {
            
        }
        private static ParseException NotAnAssignmentNodeException => new ParseException("Not an assignment");

        public static Either<ParseException, Pair<List<IToken>, AssignmentNode>> Parse(List<IToken> tokens)
        {
            Console.WriteLine("AssignmentNode");
            var maybeModifiablePrimary = ModifiablePrimaryNode.Parse(tokens);
            if (maybeModifiablePrimary.IsLeft)
                return maybeModifiablePrimary.LeftToList()[0];
            tokens = maybeModifiablePrimary.RightToList()[0].First;
            
            if (tokens.Count < 1)
                return NotAnAssignmentNodeException;

            if (!(tokens[0] is AssignmentOperatorToken))
                return NotAnAssignmentNodeException;
            tokens = tokens.Skip(1).ToList();

            var maybeExpression = ExpressionNode.Parse(tokens);
            if (maybeExpression.IsLeft)
                return maybeExpression.LeftToList()[0];
            tokens = maybeExpression.RightToList()[0].First;
            
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                    tokens[0] is SemicolonSymbolToken)
                    tokens = tokens.Skip(1).ToList();
                else break;

            return new Pair<List<IToken>, AssignmentNode>(tokens, new AssignmentNode());
        }
    }
}