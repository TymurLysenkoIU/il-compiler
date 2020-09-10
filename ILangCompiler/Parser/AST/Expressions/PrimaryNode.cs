﻿using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Literals;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Expressions
{
    public class PrimaryNode
    {
        private PrimaryNode()
        {

        }

        private static ParseException NotAPrimaryException => new ParseException("Not a primary");

        public static Either<ParseException, PrimaryNode> Parse(List<IToken> tokens)
        {
            if (tokens.Count < 1)
                return NotAPrimaryException;
            if ((tokens[0] is IntegerLiteralToken) || (tokens[0] is RealLiteralToken) ||
                (tokens[0] is TrueKeywordToken) || (tokens[0] is FalseKeywordToken))
            {
                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens.Skip(1).ToList();
                    else break;
                return new PrimaryNode();
            }
            
            var maybeModifiablePrimary = ModifiablePrimaryNode.Parse(tokens);
            if (maybeModifiablePrimary.IsLeft)
                return maybeModifiablePrimary.LeftToList()[0];
            return new PrimaryNode();
        }
    }

}
