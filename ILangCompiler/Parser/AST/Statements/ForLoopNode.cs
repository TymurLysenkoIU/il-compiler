using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.AST.Expressions;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.Constructions;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Statements
{
    public class ForLoopNode:IAstNode
    {
        private ForLoopNode()
        {
            
        }
        private static ParseException NotAForLoopException => new ParseException("Not a for loop");

        public static Either<ParseException, Pair<List<IToken>,ForLoopNode>> Parse(List<IToken> tokens)
        {
            Console.WriteLine("ForLoopNode");
            if (tokens.Count < 1)
                return NotAForLoopException;
            if (!(tokens[0] is ForKeywordToken))
                return NotAForLoopException;
            tokens = tokens.Skip(1).ToList();

            if (tokens.Count < 1)
                return NotAForLoopException;
            if (!(tokens[0] is IdentifierToken))
                return NotAForLoopException;
            tokens = tokens.Skip(1).ToList();

            var maybeRange = RangeNode.Parse(tokens);
            if (maybeRange.IsLeft)
                return maybeRange.LeftToList()[0];
            tokens = maybeRange.RightToList()[0].First;
            
            if (tokens.Count < 1)
                return NotAForLoopException;
            if (!(tokens[0] is LoopKeywordToken))
                return NotAForLoopException;
            tokens = tokens.Skip(1).ToList();
            
            var maybeBody = BodyNode.Parse(tokens);
            if (maybeBody.IsLeft)
                return maybeBody.LeftToList()[0];
            tokens = maybeBody.RightToList()[0].First;
            
            if (tokens.Count < 1)
                return NotAForLoopException;
            if (!(tokens[0] is EndKeywordToken))
                return NotAForLoopException;
            tokens = tokens.Skip(1).ToList();

            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                    tokens[0] is SemicolonSymbolToken)
                    tokens = tokens.Skip(1).ToList();
                else break;

            return new Pair<List<IToken>,ForLoopNode>(tokens, new ForLoopNode());
        }
    }
}