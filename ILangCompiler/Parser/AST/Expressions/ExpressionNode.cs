using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.LogicalOperations;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Expressions
{
    public class ExpressionNode : ITypeNode
    {
        private ExpressionNode()
        {
            
        }
        private static ParseException NotAnExpressionException => new ParseException("Not an expression");

        public static Either<ParseException, ExpressionNode> Parse(List<IToken> tokens)
        {
            var maybeRelation = RelationNode.Parse(tokens);
            if (maybeRelation.IsLeft)
                return maybeRelation.LeftToList()[0];
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                    tokens.Skip(1).ToList();
                else break;
            while (true)
            {
                if (tokens.Count < 1)
                    break;
                if (!(tokens[0] is AndKeywordToken) || !(tokens[0] is OrKeywordToken) ||
                    !(tokens[0] is XorKeywordToken))
                    break;
                tokens.Skip(1).ToList();
                var maybe2Relation = RelationNode.Parse(tokens);
                if (maybe2Relation.IsLeft)
                    return maybe2Relation.LeftToList()[0];
                
                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens.Skip(1).ToList();
                    else break;
                
            }
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                    tokens[0] is SemicolonSymbolToken)
                    tokens.Skip(1).ToList();
                else break;
            return new ExpressionNode();
        }

    }
}