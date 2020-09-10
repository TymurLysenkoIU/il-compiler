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
        public IdentifierToken Identifier { get; }
        public Option<TypeNode> Type { get;}
        public Option<ExpressionNode> Expression { get; }
        
        private static ParseException NotAVariableDeclarationException => new ParseException("Not a variable declaration");

        private VariableDeclarationNode(IdentifierToken identifier, Option<TypeNode> type, Option<ExpressionNode> expression)
        {
            Identifier = identifier;
            Type = type;
            Expression = expression;
        }

        private VariableDeclarationNode()
        {
            
        }
        

        public static Either<ParseException, Pair<List<IToken>, VariableDeclarationNode>> Parse(List<IToken> tokens)
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
                
                return new Pair<List<IToken>, VariableDeclarationNode>(tokens, new VariableDeclarationNode());
                //return new VariableDeclarationNode(identifier, Option<TypeNode>.None, maybeExpression);
            }
    
            else if (tokens[0] is ColonSymbolToken)
            {
                tokens = tokens.Skip(1).ToList();
                
                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;

                var maybeType = TypeNode.Parse(tokens);
                if (maybeType.IsLeft)
                    return maybeType.LeftToList()[0];
                tokens = maybeType.RightToList()[0].First;
                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;
                
                if (!(tokens[0] is IsKeywordToken))
                    return new Pair<List<IToken>, VariableDeclarationNode>(tokens, new VariableDeclarationNode());
                    //return new VariableDeclarationNode(identifier, maybeType, Option<ExpressionNode>.None);

                
                tokens = tokens.Skip(1).ToList();

                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;
            
            
                var maybeExpression = ExpressionNode.Parse(tokens);
            
                if (maybeExpression.IsLeft)
                    return maybeExpression.LeftToList()[0];
                tokens = maybeExpression.RightToList()[0].First;
                return new Pair<List<IToken>, VariableDeclarationNode>(tokens, new VariableDeclarationNode());
                //return new VariableDeclarationNode(identifier, maybeType, maybeExpression);
            }

            return NotAVariableDeclarationException;
        }
    }
}