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
    public class WhileLoopNode:IAstNode
    {
        public ExpressionNode Expression;
        public BodyNode Body;
        public SymT SymbolTable;
        private WhileLoopNode(ExpressionNode expression, BodyNode body, SymT symT)
        {
            Expression = expression;
            Body = body;
            SymbolTable = symT;
        }
        private static ParseException NotAWhileException => new ParseException("Not a while");

        public static Either<ParseException, Pair<List<IToken>,WhileLoopNode>> Parse(List<IToken> tokens, SymT symT)
        {
            Console.WriteLine("WhileLoopNode");
            if (tokens.Count < 1)
                return NotAWhileException;
            if (!(tokens[0] is WhileKeywordToken))
                return NotAWhileException;
            tokens = tokens.Skip(1).ToList();

            var maybeExpression = ExpressionNode.Parse(tokens);
            if (maybeExpression.IsLeft)
                return NotAWhileException;
            tokens = maybeExpression.RightToList()[0].First;
            
            if (tokens.Count < 1)
                return NotAWhileException;
            if (!(tokens[0] is LoopKeywordToken))
                return NotAWhileException;
            tokens = tokens.Skip(1).ToList();


            SymT NewSymT = new SymT(symT);
            var maybeBody = BodyNode.Parse(tokens, NewSymT);
            if (maybeBody.IsLeft)
                return NotAWhileException;
            tokens = maybeBody.RightToList()[0].First;
            
            if (tokens.Count < 1)
                return NotAWhileException;
            if (!(tokens[0] is EndKeywordToken))
                return NotAWhileException;
            tokens = tokens.Skip(1).ToList();

            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                    tokens[0] is SemicolonSymbolToken)
                    tokens = tokens.Skip(1).ToList();
                else break;

            return new Pair<List<IToken>,WhileLoopNode>(tokens, new WhileLoopNode(
                maybeExpression.RightToList()[0].Second,maybeBody.RightToList()[0].Second, NewSymT));
        }

    }
}