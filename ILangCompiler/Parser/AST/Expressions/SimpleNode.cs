using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.LogicalOperations;
using ILangCompiler.Scanner.Tokens.Predefined.Operators;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Expressions
{
    public class SimpleNode:IAstNode
    {
        private FactorNode Factor;
        private List<IToken> Tokens;
        private List<FactorNode> Factors;
        private SimpleNode(FactorNode factor, List<IToken> tokens, List<FactorNode> factors)
        {
            Factor = factor;
            Tokens = tokens;
            Factors = factors;
        }
        private static ParseException NotASimpleException => new ParseException("Not a simple");

        public static Either<ParseException, Pair<List<IToken>,SimpleNode>> Parse(List<IToken> tokens)
        {
            FactorNode factor;
            List<IToken> tokenlist = new List<IToken>();
            List<FactorNode> factors = new List<FactorNode>();
            
            Console.WriteLine("SimpleNode");
            var maybeFactor = FactorNode.Parse(tokens);
            if (maybeFactor.IsLeft)
                return maybeFactor.LeftToList()[0];
            
            var factor = maybeFactor.RightToList()[0].Second;
            var tokenlist = new List<IToken>(); 
            var factors = new List<FactorNode>();
            
            tokens = maybeFactor.RightToList()[0].First;
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                    tokens = tokens.Skip(1).ToList();
                else break;
            while (true)
            {
                if (tokens.Count < 1)
                    break;
                if (!(tokens[0] is MultiplyOperatorToken || tokens[0] is DivideOperatorToken ||
                    tokens[0] is ModOperatorToken))
                    break;
                tokens = tokens.Skip(1).ToList();
                var maybeFactor2 = FactorNode.Parse(tokens);
                if (maybeFactor2.IsLeft)
                    return maybeFactor2.LeftToList()[0];
                tokens = maybeFactor2.RightToList()[0].First;
                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;
                
            }
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                    tokens = tokens.Skip(1).ToList();
                else break;
            return new Pair<List<IToken>,SimpleNode>(tokens, new SimpleNode(factor, tokenlist, factors));
        }
    }
}