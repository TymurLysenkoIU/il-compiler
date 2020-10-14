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
    public class SimpleNode:IAstNode, ITypeTable<IEntityType>
    {
        private FactorNode Factor;
        private List<IToken> Tokens;
        private List<FactorNode> Factors;

        #region Type table

        private IDictionary<string, IEntityType> ScopeTypeTable = new Dictionary<string, IEntityType>();
        private readonly IScopedTable<IEntityType, string> ParentTypeTable;

        IDictionary<string, IEntityType> IScopedTable<IEntityType, string>.Table => ScopeTypeTable;

        Option<IScopedTable<IEntityType, string>> IScopedTable<IEntityType, string>.ParentTable => Some(ParentTypeTable);

        #endregion

        private SimpleNode(FactorNode factor, List<IToken> tokens, List<FactorNode> factors, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Factor = factor;
            Tokens = tokens;
            Factors = factors;
            ParentTypeTable = parentTypeTable;
        }
        private static ParseException NotASimpleException => new ParseException("Not a simple");

        public static Either<ParseException, Pair<List<IToken>,SimpleNode>> Parse(List<IToken> tokens, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Console.WriteLine("SimpleNode");
            var maybeFactor = FactorNode.Parse(tokens, parentTypeTable);
            if (maybeFactor.IsLeft)
                return maybeFactor.LeftToList()[0];

            var factor = maybeFactor.RightToList()[0].Second;
            var tokenlist = new List<IToken>();
            var factors = new List<FactorNode>();

            tokens = maybeFactor.RightToList()[0].First;
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                    tokens = tokens.Skip(1).ToList();
                else break;
            while (true)
            {
                if (tokens.Count < 1)
                    break;
                if (!(tokens[0] is MultiplyOperatorToken || tokens[0] is DivideOperatorToken ||
                    tokens[0] is ModOperatorToken))
                    break;
                tokens = tokens.Skip(1).ToList();
                var maybeFactor2 = FactorNode.Parse(tokens, parentTypeTable);
                if (maybeFactor2.IsLeft)
                    return maybeFactor2.LeftToList()[0];
                tokens = maybeFactor2.RightToList()[0].First;
                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;

            }
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                    tokens = tokens.Skip(1).ToList();
                else break;
            return new Pair<List<IToken>,SimpleNode>(tokens, new SimpleNode(factor, tokenlist, factors, parentTypeTable));
        }
    }
}