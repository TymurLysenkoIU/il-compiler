using System;
using System.Collections.Generic;
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
    public class PrimaryNode : IAstNode
    {
        private IToken Token;
        private ModifiablePrimaryNode ModifiablePrimary;

        private PrimaryNode(IToken token, ModifiablePrimaryNode modifiablePrimary)
        {
            Token = token;
            ModifiablePrimary = modifiablePrimary;
        }

        private PrimaryNode(IToken token)
        {
            Token = token;
        }

        private static ParseException NotAPrimaryException => new ParseException("Not a primary");

        public static Either<ParseException, Pair<List<IToken>, PrimaryNode>> Parse(List<IToken> tokens)
        {
            IToken token = null;
            Console.WriteLine("PrimaryNode");
            if (tokens.Count < 1)
                return NotAPrimaryException;
            if ((tokens[0] is IntegerLiteralToken) || (tokens[0] is RealLiteralToken) ||
                (tokens[0] is TrueKeywordToken) || (tokens[0] is FalseKeywordToken))
            {
                token = tokens[0];
                tokens = tokens.Skip(1).ToList();
                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;
                return new Pair<List<IToken>, PrimaryNode>(tokens, new PrimaryNode(token));
            }

            var maybeModifiablePrimary = ModifiablePrimaryNode.Parse(tokens);
            if (maybeModifiablePrimary.IsLeft)
                return maybeModifiablePrimary.LeftToList()[0];
            var modifiablePrimary = maybeModifiablePrimary.RightToList()[0].Second;
            tokens = maybeModifiablePrimary.RightToList()[0].First;
            return new Pair<List<IToken>, PrimaryNode>(tokens, new PrimaryNode(token, modifiablePrimary));
        }
    }
}