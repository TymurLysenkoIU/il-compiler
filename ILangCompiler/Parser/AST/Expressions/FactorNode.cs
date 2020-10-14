using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Operators;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Expressions
{
    public class FactorNode: IAstNode
    {
        private SummandNode Summand;
        private List<IToken> Tokens;
        private List<SummandNode> Summands;

        private FactorNode(SummandNode summand, List<IToken> tokens, List<SummandNode> summands)
        {
            Summand = summand;
            Tokens = tokens;
            Summands = summands;
        }

        private static ParseException NotAFactorException => new ParseException("Not a factor");

        public static Either<ParseException, Pair<List<IToken>,FactorNode>> Parse(List<IToken> tokens)
        {
            Console.WriteLine("FactorNode");
            var maybeSummand = SummandNode.Parse(tokens);
            if (maybeSummand.IsLeft)
                return maybeSummand.LeftToList()[0];
            var summand = maybeSummand.RightToList()[0].Second;
            var tokenlist = new List<IToken>();
            var summands = new List<SummandNode>();
            tokens = maybeSummand.RightToList()[0].First;
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                    tokens = tokens.Skip(1).ToList();
                else break;
            while (true)
            {
                if (tokens.Count < 1)
                    break;
                if (!((tokens[0] is PlusOperatorToken) || (tokens[0] is MinusOperatorToken)))
                    break;
                tokenlist.Add(tokens[0]);
                tokens = tokens.Skip(1).ToList();
                var maybeSummand2 = SummandNode.Parse(tokens);
                if (maybeSummand2.IsLeft)
                    return maybeSummand2.LeftToList()[0];
                tokens = maybeSummand2.RightToList()[0].First;
                summands.Add(maybeSummand2.RightToList()[0].Second);
                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;

            }
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                    tokens = tokens.Skip(1).ToList();
                else break;
            return new Pair<List<IToken>,FactorNode>(tokens, new FactorNode(summand,tokenlist,summands));
        }
    }
}