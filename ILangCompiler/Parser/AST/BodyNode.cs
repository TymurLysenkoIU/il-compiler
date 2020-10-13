using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.Parser.AST.Expressions;
using ILangCompiler.Parser.AST.Statements;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.Constructions;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST
{
    public class BodyNode : IAstNode
    {
        public ImmutableArray<IAstNode> Elements;
        public ExpressionNode ReturnExpressionNode;

        private BodyNode(ImmutableArray<IAstNode> elements)
        {
            Elements = elements;
        }
        private BodyNode(ImmutableArray<IAstNode> elements, ExpressionNode returnExpressionNode)
        {
            Elements = elements;
            ReturnExpressionNode = returnExpressionNode;
        }

        private static ParseException NotABodyException => new ParseException("Not a body");

        public static Either<ParseException, Pair<List<IToken>,BodyNode>> Parse(List<IToken> tokens)
        {
            Console.WriteLine("BodyNode");

            ImmutableArray<IAstNode> elements = ImmutableArray<IAstNode>.Empty;

            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                    tokens[0] is SemicolonSymbolToken)
                    tokens = tokens.Skip(1).ToList();
                else break;

            while (true)
            {
                var maybeSimpleDeclaration1 = VariableDeclarationNode.Parse(tokens);
                if (maybeSimpleDeclaration1.IsRight)
                {
                    tokens = maybeSimpleDeclaration1.RightToList()[0].First;
                    elements = elements.Add(maybeSimpleDeclaration1.RightToList()[0].Second);
                    while (tokens.Count > 0)
                        if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                            tokens[0] is SemicolonSymbolToken)
                            tokens = tokens.Skip(1).ToList();
                        else break;
                    continue;
                }

                var maybeSimpleDeclaration2 = TypeDeclarationNode.Parse(tokens);
                if (maybeSimpleDeclaration2.IsRight)
                {
                    tokens = maybeSimpleDeclaration2.RightToList()[0].First;
                    elements = elements.Add(maybeSimpleDeclaration2.RightToList()[0].Second);
                    while (tokens.Count > 0)
                        if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                            tokens[0] is SemicolonSymbolToken)
                            tokens = tokens.Skip(1).ToList();
                        else break;
                    continue;
                }

                var maybeStatement = StatementNode.Parse(tokens);
                if (maybeStatement.IsRight)
                {
                    tokens = maybeStatement.RightToList()[0].First;
                    elements = elements.Add(maybeStatement.RightToList()[0].Second);
                    while (tokens.Count > 0)
                        if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                            tokens[0] is SemicolonSymbolToken)
                            tokens = tokens.Skip(1).ToList();
                        else break;
                    continue;
                }
                
                var maybeExpression = StatementNode.Parse(tokens);
                if (maybeExpression.IsRight)
                {
                    tokens = maybeExpression.RightToList()[0].First;
                    elements = elements.Add(maybeExpression.RightToList()[0].Second);
                    while (tokens.Count > 0)
                        if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                            tokens[0] is SemicolonSymbolToken)
                            tokens = tokens.Skip(1).ToList();
                        else break;
                    continue;
                }

                break;
            }
            
            
            if (tokens.Count < 1)
                return NotABodyException;
            if (!(tokens[0] is ReturnKeywordToken))
                return NotABodyException;
            tokens = tokens.Skip(1).ToList();
            var maybeExpression1 = ExpressionNode.Parse(tokens);
            if (maybeExpression1.IsRight)
            {
                
                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                        tokens[0] is SemicolonSymbolToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;
                tokens = maybeExpression1.RightToList()[0].First;
                return new Pair<List<IToken>,BodyNode>(tokens, new BodyNode(elements, maybeExpression1.RightToList()[0].Second));

            }  
            
            
            
            return new Pair<List<IToken>,BodyNode>(tokens, new BodyNode(elements));
        }

    }
}