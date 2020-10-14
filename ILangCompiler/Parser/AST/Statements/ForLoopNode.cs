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
        public IdentifierToken Identifier;
        public RangeNode Range;
        public BodyNode Body;
        public SymT SymbolTable;
        private ForLoopNode(IdentifierToken identifier, RangeNode range, BodyNode body, SymT symT)
        {
            Identifier = identifier;
            Range = range;
            Body = body;
            SymbolTable = symT;
        }
        private static ParseException NotAForLoopException => new ParseException("Not a for loop");
        private static ParseException RepeatedIdentifierException => new ParseException("Repeating identifier in the same scope");
        public static Either<ParseException, Pair<List<IToken>,ForLoopNode>> Parse(List<IToken> tokens, SymT symT)
        {
            Console.WriteLine("ForLoopNode");
            SymT NewSymT = new SymT(symT);
            
            if (tokens.Count < 1)
                return NotAForLoopException;
            if (!(tokens[0] is ForKeywordToken))
                return NotAForLoopException;
            tokens = tokens.Skip(1).ToList();

            if (tokens.Count < 1)
                return NotAForLoopException;
            if (!(tokens[0] is IdentifierToken))
                return NotAForLoopException;
            IdentifierToken identifier = (IdentifierToken) tokens[0]; 
            tokens = tokens.Skip(1).ToList();

            if (NewSymT.Contain(identifier))
            {
                
                Console.Write("    Error : Repeating identifier in the same scope\n");
                return RepeatedIdentifierException;
            }
            else
            {
                NewSymT.Add(identifier);
            }
            
            var maybeRange = RangeNode.Parse(tokens);
            if (maybeRange.IsLeft)
                return maybeRange.LeftToList()[0];
            RangeNode range = maybeRange.RightToList()[0].Second;
            tokens = maybeRange.RightToList()[0].First;
            
            if (tokens.Count < 1)
                return NotAForLoopException;
            if (!(tokens[0] is LoopKeywordToken))
                return NotAForLoopException;
            tokens = tokens.Skip(1).ToList();
            
            var maybeBody = BodyNode.Parse(tokens, NewSymT);
            if (maybeBody.IsLeft)
                return maybeBody.LeftToList()[0];
            BodyNode body = maybeBody.RightToList()[0].Second;
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

            return new Pair<List<IToken>,ForLoopNode>(tokens, new ForLoopNode(
                identifier, range, body, NewSymT));
        }
    }
}