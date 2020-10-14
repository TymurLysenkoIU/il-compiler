using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.Parser.AST.TypeTable;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.LogicalOperations;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ILangCompiler.Parser.AST.Expressions
{
    public class ExpressionNode : ITypeNode, ITypeTable<IEntityType>
    {
        private RelationNode Relation1;
        private List<RelationNode> Relations;
        private List<IToken> Tokens;

        #region Type table

        private IDictionary<string, IEntityType> ScopeTypeTable = new Dictionary<string, IEntityType>();
        private readonly IScopedTable<IEntityType, string> ParentTypeTable;

        IDictionary<string, IEntityType> IScopedTable<IEntityType, string>.Table => ScopeTypeTable;

        Option<IScopedTable<IEntityType, string>> IScopedTable<IEntityType, string>.ParentTable => Some(ParentTypeTable);

        #endregion


        private ExpressionNode(RelationNode relation1, List<RelationNode> relations, List<IToken> tokens, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Relation1 = relation1;
            Relations = relations;
            Tokens = tokens;
            ParentTypeTable = parentTypeTable;
        }
        private static ParseException NotAnExpressionException => new ParseException("Not an expression");

        public static Either<ParseException, Pair<List<IToken>, ExpressionNode>> Parse(List<IToken> tokens, IScopedTable<IEntityType, string> parentTypeTable)
        {

            var relations = new List<RelationNode>();
            var tokenList = new List<IToken>();


            Console.WriteLine("ExpressionNode");
            var maybeRelation = RelationNode.Parse(tokens, parentTypeTable);
            if (maybeRelation.IsLeft)
                return maybeRelation.LeftToList()[0];
            tokens = maybeRelation.RightToList()[0].First;
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                    tokens = tokens.Skip(1).ToList();
                else break;

            while (true)
            {
                if (tokens.Count < 1)
                    break;
                if (!(tokens[0] is AndKeywordToken || tokens[0] is OrKeywordToken ||
                    tokens[0] is XorKeywordToken))
                    break;

                tokenList.Add(tokens[0]);

                tokens = tokens.Skip(1).ToList();
                var maybe2Relation = RelationNode.Parse(tokens, parentTypeTable);
                if (maybe2Relation.IsLeft)
                    return maybe2Relation.LeftToList()[0];
                relations.Add(maybe2Relation.RightToList()[0].Second);
                tokens = maybe2Relation.RightToList()[0].First;
                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;

            }
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                    tokens[0] is SemicolonSymbolToken)
                    tokens = tokens.Skip(1).ToList();
                else break;
            return new Pair<List<IToken>, ExpressionNode> (
                tokens,
                new ExpressionNode(maybeRelation.RightToList()[0].Second,relations,tokenList, parentTypeTable)
            );
        }

    }
}