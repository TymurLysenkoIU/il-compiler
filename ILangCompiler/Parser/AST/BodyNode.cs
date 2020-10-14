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
        public ImmutableArray<IAstNode> Elements;
        
        private BodyNode(ImmutableArray<IAstNode> elements)
        {
            Elements = elements;
        }

        private static ParseException NotABodyException => new ParseException("Not a body");

        public static Either<ParseException, Pair<List<IToken>,BodyNode>> Parse(List<IToken> tokens, SymT symT)
        {
            Console.WriteLine("BodyNode");
            
            ImmutableArray<IAstNode> elements = new ImmutableArray<IAstNode>();
            
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                    tokens[0] is SemicolonSymbolToken)
                    tokens = tokens.Skip(1).ToList();
                else break;
            
            while (true)
            {
                var maybeSimpleDeclaration1 = VariableDeclarationNode.Parse(tokens, symT);
                if (maybeSimpleDeclaration1.IsRight)
                {
                    tokens = maybeSimpleDeclaration1.RightToList()[0].First;
                    elements.Add(maybeSimpleDeclaration1.RightToList()[0].Second);
                    while (tokens.Count > 0)
                        if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                            tokens[0] is SemicolonSymbolToken)
                            tokens = tokens.Skip(1).ToList();
                        else break;
                    continue;
                }

                var maybeSimpleDeclaration2 = TypeDeclarationNode.Parse(tokens, symT);
                if (maybeSimpleDeclaration2.IsRight)
                {
                    tokens = maybeSimpleDeclaration2.RightToList()[0].First;
                    elements.Add(maybeSimpleDeclaration2.RightToList()[0].Second);
                    while (tokens.Count > 0)
                        if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                            tokens[0] is SemicolonSymbolToken)
                            tokens = tokens.Skip(1).ToList();
                        else break;
                    continue;
                }

                var maybeStatement = StatementNode.Parse(tokens, symT);
                if (maybeStatement.IsRight)
                {
                    tokens = maybeStatement.RightToList()[0].First;
                    elements.Add(maybeStatement.RightToList()[0].Second);
                    while (tokens.Count > 0)
                        if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                            tokens[0] is SemicolonSymbolToken)
                            tokens = tokens.Skip(1).ToList();
                        else break;
                    continue;
                }
                
                break;
            }
            return new Pair<List<IToken>,BodyNode>(tokens, new BodyNode(elements));
        }

    }
}