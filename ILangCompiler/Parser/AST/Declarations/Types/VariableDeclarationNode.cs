using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.AST.Expressions;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Declarations.Types
{
    public class VariableDeclarationNode: ISimpleDeclarationNode
    {
        public IdentifierToken Identifier;
        public Option<TypeNode> Type;
        public Option<ExpressionNode> Expression;
        
        private static ParseException NotAVariableDeclarationException => new ParseException("Not a variable declaration");
        private static ParseException RepeatedIdentifierException => new ParseException("Repeating identifier in the same scope");
        private VariableDeclarationNode(IdentifierToken identifier, Option<TypeNode> type, Option<ExpressionNode> expression)
        {
            Identifier = identifier;
            Type = type;
            Expression = expression;
        }

        public static Either<ParseException, Pair<List<IToken>, VariableDeclarationNode>> Parse(List<IToken> tokens, SymT symT)
        {
            Console.WriteLine("VariableDeclarationNode");
            if (tokens.Count < 3)
                return NotAVariableDeclarationException;
            var maybeVar = tokens[0];
            var maybeIdentifier = tokens[1];
            var maybeColonOrIs = tokens[2];
            if (
                !(maybeVar is VarKeywordToken) ||
                !(maybeIdentifier is IdentifierToken) ||
                !(maybeColonOrIs is ColonSymbolToken || maybeColonOrIs is IsKeywordToken)
            )
                return NotAVariableDeclarationException;
            

            
            IdentifierToken identifier = (IdentifierToken) maybeIdentifier;
            tokens = tokens.Skip(2).ToList();

            if (tokens[0] is IsKeywordToken)
            {
                tokens = tokens.Skip(1).ToList();

                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;
                
                
                var maybeExpression = ExpressionNode.Parse(tokens);
                
                if (maybeExpression.IsLeft)
                    return maybeExpression.LeftToList()[0];
                tokens = maybeExpression.RightToList()[0].First;
                
                if (symT.Contain(identifier))
                {
                    Console.Write("    Error : Repeating identifier in the same scope\n");
                    return RepeatedIdentifierException;
                }
                else
                {
                    symT.Add(identifier);
                }
                
                return new Pair<List<IToken>, VariableDeclarationNode>(tokens, new VariableDeclarationNode(
                    identifier, Option<TypeNode>.None, maybeExpression.RightToList()[0].Second));
                //return new VariableDeclarationNode(identifier, Option<TypeNode>.None, maybeExpression);
            }
    
            else if (tokens[0] is ColonSymbolToken)
            {
                tokens = tokens.Skip(1).ToList();
                
                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;

                var maybeType = TypeNode.Parse(tokens, symT);
                if (maybeType.IsLeft)
                    return maybeType.LeftToList()[0];
                tokens = maybeType.RightToList()[0].First;
                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;

                if (!(tokens[0] is IsKeywordToken))
                {
                    if (symT.Contain(identifier))
                    {
                        Console.WriteLine("Repeating identifier in the same scope");
                    }
                    else
                    {
                        symT.Add(identifier);
                    }
                    return new Pair<List<IToken>, VariableDeclarationNode>(tokens, new VariableDeclarationNode(
                        identifier, maybeType.RightToList()[0].Second, Option<ExpressionNode>.None));
                }

                tokens = tokens.Skip(1).ToList();

                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;
            
            
                var maybeExpression = ExpressionNode.Parse(tokens);
            
                if (maybeExpression.IsLeft)
                    return maybeExpression.LeftToList()[0];
                tokens = maybeExpression.RightToList()[0].First;
                
                if (symT.Contain(identifier))
                {
                    Console.WriteLine("Repeating identifier in the same scope");
                }
                else
                {
                    symT.Add(identifier);
                }
                
                return new Pair<List<IToken>, VariableDeclarationNode>(tokens, new VariableDeclarationNode(
                    identifier, maybeType.RightToList()[0].Second, maybeExpression.RightToList()[0].Second));
            }

            return NotAVariableDeclarationException;
        }
    }
}