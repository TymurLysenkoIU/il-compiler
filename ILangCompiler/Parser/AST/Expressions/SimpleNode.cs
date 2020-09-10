﻿using System;
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
        private SimpleNode()
        {
            
        }
        private static ParseException NotASimpleException => new ParseException("Not a simple");

        public static Either<ParseException, SimpleNode> Parse(List<IToken> tokens)
        {
            Console.WriteLine("SimpleNode");
            var maybeFactor = FactorNode.Parse(tokens);
            if (maybeFactor.IsLeft)
                return maybeFactor.LeftToList()[0];
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                    tokens.Skip(1).ToList();
                else break;
            while (true)
            {
                if (tokens.Count < 1)
                    break;
                if (!(tokens[0] is MultiplyOperatorToken) || !(tokens[0] is DivideOperatorToken) ||
                    !(tokens[0] is ModOperatorToken))
                    break;
                tokens.Skip(1).ToList();
                var maybeFactor2 = FactorNode.Parse(tokens);
                if (maybeFactor2.IsLeft)
                    return maybeFactor2.LeftToList()[0];
                
                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens.Skip(1).ToList();
                    else break;
                
            }
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                    tokens.Skip(1).ToList();
                else break;
            return new SimpleNode();
        }
    }
}