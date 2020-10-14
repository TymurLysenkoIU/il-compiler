using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.AST.TypeTable;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Operators;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ILangCompiler.Parser.AST.Expressions
{
    public class FactorNode: IAstNode, ITypeTable<IEntityType>
    {
        private SummandNode Summand;
        private List<IToken> Tokens;
        private List<SummandNode> Summands;

        #region Type table

        private IDictionary<string, IEntityType> ScopeTypeTable = new Dictionary<string, IEntityType>();
        private readonly IScopedTable<IEntityType, string> ParentTypeTable;

        IDictionary<string, IEntityType> IScopedTable<IEntityType, string>.Table => ScopeTypeTable;

        Option<IScopedTable<IEntityType, string>> IScopedTable<IEntityType, string>.ParentTable => Some(ParentTypeTable);

        #endregion

        private FactorNode(SummandNode summand, List<IToken> tokens, List<SummandNode> summands, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Summand = summand;
            Tokens = tokens;
            Summands = summands;
            ParentTypeTable = parentTypeTable;
        }

        private static ParseException NotAFactorException => new ParseException("Not a factor");

        public static Either<ParseException, Pair<List<IToken>,FactorNode>> Parse(List<IToken> tokens, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Console.WriteLine("FactorNode");
            var maybeSummand = SummandNode.Parse(tokens, parentTypeTable);
            if (maybeSummand.IsLeft)
                return maybeSummand.LeftToList()[0];
            var summand = maybeSummand.RightToList()[0].Second;
            var tokenlist = new List<IToken>();
            var summands = new List<SummandNode>();
            tokens = maybeSummand.RightToList()[0].First;
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                    tokens = tokens.Skip(1).ToList();
                else break;
            while (true)
            {
                if (tokens.Count < 1)
                    break;
                if (!((tokens[0] is PlusOperatorToken) || (tokens[0] is MinusOperatorToken)))
                    break;
                tokenlist.Add(tokens[0]);
                tokens = tokens.Skip(1).ToList();
                var maybeSummand2 = SummandNode.Parse(tokens, parentTypeTable);
                if (maybeSummand2.IsLeft)
                    return maybeSummand2.LeftToList()[0];
                tokens = maybeSummand2.RightToList()[0].First;
                summands.Add(maybeSummand2.RightToList()[0].Second);
                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;

            }
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                    tokens = tokens.Skip(1).ToList();
                else break;
            return new Pair<List<IToken>,FactorNode>(tokens, new FactorNode(summand,tokenlist,summands, parentTypeTable));
        }
    }
}