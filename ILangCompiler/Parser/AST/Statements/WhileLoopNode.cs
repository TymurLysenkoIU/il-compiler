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
        private WhileLoopNode()
        {
            
        }
        private static ParseException NotAWhileException => new ParseException("Not a while");

        public static Either<ParseException, WhileLoopNode> Parse(List<IToken> tokens)
        {
            Console.WriteLine("WhileLoopNode");
            if (tokens.Count < 1)
                return NotAWhileException;
            if (!(tokens[0] is WhileKeywordToken))
                return NotAWhileException;
            tokens.Skip(1).ToList();

            var maybeExpression = ExpressionNode.Parse(tokens);
            if (maybeExpression.IsLeft)
                return NotAWhileException;
            
            if (tokens.Count < 1)
                return NotAWhileException;
            if (!(tokens[0] is LoopKeywordToken))
                return NotAWhileException;
            tokens.Skip(1).ToList();
            
            var maybeBody = BodyNode.Parse(tokens);
            if (maybeBody.IsLeft)
                return NotAWhileException;
            
            
            if (tokens.Count < 1)
                return NotAWhileException;
            if (!(tokens[0] is EndKeywordToken))
                return NotAWhileException;
            tokens.Skip(1).ToList();

            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                    tokens[0] is SemicolonSymbolToken)
                    tokens.Skip(1).ToList();
                else break;

            return new WhileLoopNode();
        }

    }
}