using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Expressions
{
    public class SummandNode
    {
        private SummandNode()
        {
            
        }

        private static ParseException NotASummandException => new ParseException("Not a summand");

        public static Either<ParseException, SummandNode> Parse(List<IToken> tokens)
        {
            var maybePrimary = SummandNode.Parse(tokens);
            if (maybePrimary.IsRight)
            {
                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens.Skip(1).ToList();
                    else break;
                return new SummandNode();
            }
            
            if (tokens.Count < 1)
                return NotASummandException;
            if (!(tokens[0] is LeftParenthSymbolToken))
                return NotASummandException;
            tokens.Skip(1).ToList();

            var maybeExpression = ExpressionNode.Parse(tokens);
            if (maybeExpression.IsLeft)
                return maybeExpression.LeftToList()[0];
            
            if (tokens.Count < 1)
                return NotASummandException;
            if (!(tokens[0] is RightParenthSymbolToken))
                return NotASummandException;
            tokens.Skip(1).ToList();
            
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                    tokens.Skip(1).ToList();
                else break;
            
            return new SummandNode();
        }

    }
}