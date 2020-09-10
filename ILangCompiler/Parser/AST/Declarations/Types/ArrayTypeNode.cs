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
    public class ArrayTypeNode : ITypeNode
    {
        private ArrayTypeNode()
        {
            
        }

        private static ParseException NotAnArrayTypeException => new ParseException("Not an array type");

        public static Either<ParseException, Pair<List<IToken>,ArrayTypeNode>> Parse(List<IToken> tokens)
        {
            Console.WriteLine("ArrayTypeNode");
            if (tokens.Count < 2)
                return NotAnArrayTypeException;
            if (!(tokens[0] is ArrayKeywordToken) || !(tokens[1] is LeftBracketSymbolToken))
                return NotAnArrayTypeException;
            tokens = tokens.Skip(2).ToList();
            var maybeExpression = ExpressionNode.Parse(tokens);
            if (maybeExpression.IsLeft)
            {
                if (tokens.Count < 1)
                    return NotAnArrayTypeException;
                if (!(tokens[0] is RightBracketSymbolToken))
                    return NotAnArrayTypeException;
                tokens = tokens.Skip(1).ToList();
                var maybeType1 = TypeNode.Parse(tokens);
                if (maybeType1.IsLeft)
                    return maybeType1.LeftToList()[0];
                tokens = maybeType1.RightToList()[0].First;
                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                        tokens[0] is SemicolonSymbolToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;
                return new Pair <List<IToken>, ArrayTypeNode>(tokens, new ArrayTypeNode());

            }

            tokens = maybeExpression.RightToList()[0].First;
                
            if (tokens.Count < 1)
                return NotAnArrayTypeException;
            if (!(tokens[0] is RightBracketSymbolToken))
                return NotAnArrayTypeException;
            tokens = tokens.Skip(1).ToList();
            var maybeType2 = TypeNode.Parse(tokens);
            if (maybeType2.IsLeft)
                return maybeType2.LeftToList()[0];
            tokens = maybeType2.RightToList()[0].First;
            
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                    tokens[0] is SemicolonSymbolToken)
                    tokens = tokens.Skip(1).ToList();
                else break;
            return new Pair <List<IToken>, ArrayTypeNode>(tokens, new ArrayTypeNode());
            

        }
    }
}