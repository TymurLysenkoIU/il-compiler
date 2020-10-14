using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.AST.TypeTable;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.LogicalOperations;
using ILangCompiler.Scanner.Tokens.Predefined.Operators;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ILangCompiler.Parser.AST.Expressions
{
    public class RelationNode:IAstNode, ITypeTable<IEntityType>
    {
        private SimpleNode Simple1;
        private IToken Token;
        private SimpleNode Simple2;

        #region Type table

        private IDictionary<string, IEntityType> ScopeTypeTable = new Dictionary<string, IEntityType>();
        private readonly IScopedTable<IEntityType, string> ParentTypeTable;

        IDictionary<string, IEntityType> IScopedTable<IEntityType, string>.Table => ScopeTypeTable;

        Option<IScopedTable<IEntityType, string>> IScopedTable<IEntityType, string>.ParentTable => Some(ParentTypeTable);

        #endregion

        private RelationNode(SimpleNode simple1, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Simple1 = simple1;
            ParentTypeTable = parentTypeTable;
        }
        private RelationNode(SimpleNode simple1, IToken token, SimpleNode simple2, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Simple1 = simple1;
            Token = token;
            Simple2 = simple2;
            ParentTypeTable = parentTypeTable;
        }
        private static ParseException NotARelationException => new ParseException("Not a relation");

        public static Either<ParseException, Pair<List<IToken>,RelationNode>> Parse(List<IToken> tokens, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Console.WriteLine("RelationNode");
            var maybeSimple = SimpleNode.Parse(tokens, parentTypeTable);
            if (maybeSimple.IsLeft)
                return maybeSimple.LeftToList()[0];

            var simple1 = maybeSimple.RightToList()[0].Second;
            tokens = maybeSimple.RightToList()[0].First;
            if (tokens.Count < 1)
                return new Pair<List<IToken>,RelationNode>(tokens, new RelationNode(simple1, parentTypeTable));
            if (!(tokens[0] is LeOperatorToken || tokens[0] is LtOperatorToken ||
                  tokens[0] is GeOperatorToken || tokens[0] is GtOperatorToken ||
                  tokens[0] is EqualsOperatorToken || tokens[0] is NotEqualsOperatorToken))
                return new Pair<List<IToken>,RelationNode>(tokens, new RelationNode(simple1, parentTypeTable));

            var token = tokens[0];
            tokens = tokens.Skip(1).ToList();
            var maybeSimple2 = SimpleNode.Parse(tokens, parentTypeTable);
            if (maybeSimple2.IsLeft)
                return maybeSimple2.LeftToList()[0];
            var simple2 = maybeSimple2.RightToList()[0].Second;
            tokens = maybeSimple2.RightToList()[0].First;
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                    tokens = tokens.Skip(1).ToList();
                else break;

            return new Pair<List<IToken>,RelationNode>(tokens, new RelationNode(simple1,token,simple2, parentTypeTable));

        }
    }
}