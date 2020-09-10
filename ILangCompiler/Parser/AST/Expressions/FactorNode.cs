using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Operators;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Expressions
{
    public class FactorNode
    {
        private FactorNode()
        {
            
        }
        private static ParseException NotAFactorException => new ParseException("Not a factor");

        public static Either<ParseException, FactorNode> Parse(List<IToken> tokens)
        {
            var maybeSummand = SummandNode.Parse(tokens);
            if (maybeSummand.IsLeft)
                return maybeSummand.LeftToList()[0];
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                    tokens.Skip(1).ToList();
                else break;
            while (true)
            {
                if (tokens.Count < 1)
                    break;
                if (!(tokens[0] is PlusOperatorToken) || !(tokens[0] is MinusOperatorToken))
                    break;
                tokens.Skip(1).ToList();
                var maybeSummand2 = SummandNode.Parse(tokens);
                if (maybeSummand2.IsLeft)
                    return maybeSummand2.LeftToList()[0];
                
                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens.Skip(1).ToList();
                    else break;
                
            }
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                    tokens.Skip(1).ToList();
                else break;
            return new FactorNode();
        }
    }
}