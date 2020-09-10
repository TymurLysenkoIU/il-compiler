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
    public class RelationNode
    {
        private RelationNode()
        {
            
        }
        private static ParseException NotARelationException => new ParseException("Not a relation");

        public static Either<ParseException, RelationNode> Parse(List<IToken> tokens)
        {
            var maybeSimple = SimpleNode.Parse(tokens);
            if (maybeSimple.IsLeft)
                return maybeSimple.LeftToList()[0];
            if (tokens.Count < 1)
                return new RelationNode();
            if (!(tokens[0] is LeOperatorToken) || !(tokens[0] is LtOperatorToken) ||
                !(tokens[0] is GeOperatorToken) || !(tokens[0] is GtOperatorToken) ||
                !(tokens[0] is EqualsOperatorToken) || !(tokens[0] is NotEqualsOperatorToken))
                return new RelationNode();

            tokens.Skip(1).ToList();
            var maybeSimple2 = SimpleNode.Parse(tokens);
            if (maybeSimple2.IsLeft)
                return maybeSimple2.LeftToList()[0];
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                    tokens.Skip(1).ToList();
                else break;
            
            return new RelationNode();
            
        }
    }
}