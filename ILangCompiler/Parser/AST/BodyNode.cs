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

        public static Either<ParseException, Pair<List<IToken>,BodyNode>> Parse(List<IToken> tokens)
        {
            Console.WriteLine("BodyNode");
            
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
                    while (tokens.Count > 0)
                        if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                            tokens[0] is SemicolonSymbolToken)
                            tokens = tokens.Skip(1).ToList();
                        else break;
                    continue;
                }
                
                break;
            }
            return new Pair<List<IToken>,BodyNode>(tokens, new BodyNode());
        }

    }
}