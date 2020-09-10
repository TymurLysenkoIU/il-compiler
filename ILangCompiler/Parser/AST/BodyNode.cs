using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.Parser.AST.Statements;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST
{
    public class BodyNode : IAstNode
    {
        public ImmutableArray<IBodyElementNode> Elements { get; }
        
        private BodyNode()
        {
            
        }

        private static ParseException NotABodyException => new ParseException("Not a body");

        public static Either<ParseException, BodyNode> Parse(List<IToken> tokens)
        {
            Console.WriteLine("BodyNode");
            while (true)
            {
                var maybeSimpleDeclaration1 = VariableDeclarationNode.Parse(tokens);
                if (maybeSimpleDeclaration1.IsRight)
                {
                    while (tokens.Count > 0)
                        if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                            tokens[0] is SemicolonSymbolToken)
                            tokens.Skip(1).ToList();
                        else break;
                    continue;
                }

                var maybeSimpleDeclaration2 = TypeDeclarationNode.Parse(tokens);
                if (maybeSimpleDeclaration2.IsRight)
                {
                    while (tokens.Count > 0)
                        if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                            tokens[0] is SemicolonSymbolToken)
                            tokens.Skip(1).ToList();
                        else break;
                    continue;
                }

                var maybeStatement = StatementNode.Parse(tokens);
                if (maybeStatement.IsRight)
                {
                    while (tokens.Count > 0)
                        if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                            tokens[0] is SemicolonSymbolToken)
                            tokens.Skip(1).ToList();
                        else break;
                    continue;
                }
                
                break;
            }
            return new BodyNode();
        }

    }
}