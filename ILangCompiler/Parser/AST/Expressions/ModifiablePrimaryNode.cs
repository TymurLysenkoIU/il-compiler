using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Expressions
{
    public class ModifiablePrimaryNode: IAstNode
    {
        private ModifiablePrimaryNode()
        {
            
        }

        private static ParseException NotAModifiablePrimaryException => new ParseException("Not a modifiable primary");

        public static Either<ParseException, Pair<List<IToken>,ModifiablePrimaryNode>> Parse(List<IToken> tokens)
        {
            Console.WriteLine("ModifiablePrimaryNode");
            
            if (tokens.Count < 1)
                return NotAModifiablePrimaryException;
            if (!(tokens[0] is IdentifierToken))
                return NotAModifiablePrimaryException;
            tokens = tokens.Skip(1).ToList();
            while (true)
            {
                if (tokens.Count < 1)
                    return new Pair<List<IToken>,ModifiablePrimaryNode> (tokens, new ModifiablePrimaryNode());

                if (tokens[0] is DotSymbolToken)
                {
                    tokens = tokens.Skip(1).ToList();
                    if (tokens.Count < 1)
                        return NotAModifiablePrimaryException;
                    if (!(tokens[0] is IdentifierToken))
                        return NotAModifiablePrimaryException;
                    tokens = tokens.Skip(1).ToList();
                }
                
                else if (tokens[0] is LeftBracketSymbolToken)
                {
                    tokens = tokens.Skip(1).ToList();
                    var maybeExpression = ExpressionNode.Parse(tokens);
                    if (maybeExpression.IsLeft)
                        return maybeExpression.LeftToList()[0];
                    tokens = maybeExpression.RightToList()[0].First;
                    if (tokens.Count < 1)
                        return NotAModifiablePrimaryException;
                    if (!(tokens[0] is RightBracketSymbolToken))
                        return NotAModifiablePrimaryException;
                    tokens = tokens.Skip(1).ToList();
                    
                }
                else break;

            }
            
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                    tokens[0] is SemicolonSymbolToken)
                    tokens = tokens.Skip(1).ToList();
                else break;
            return new Pair<List<IToken>,ModifiablePrimaryNode> (tokens, new ModifiablePrimaryNode());;
        }

    }
}