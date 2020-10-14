using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.Parser.AST.TypeTable;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ILangCompiler.Parser.AST.Expressions
{
    public class SummandNode:IAstNode, ITypeTable<IEntityType>
    {
        public PrimaryNode Primary;
        public ExpressionNode Expression;

        #region Type table

        private IDictionary<string, IEntityType> ScopeTypeTable = new Dictionary<string, IEntityType>();
        private readonly IScopedTable<IEntityType, string> ParentTypeTable;

        IDictionary<string, IEntityType> IScopedTable<IEntityType, string>.Table => ScopeTypeTable;

        Option<IScopedTable<IEntityType, string>> IScopedTable<IEntityType, string>.ParentTable => Some(ParentTypeTable);

        #endregion

        private SummandNode(PrimaryNode primary, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Primary = primary;
            ParentTypeTable = parentTypeTable;
        }
        private SummandNode(ExpressionNode expression, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Expression = expression;
            ParentTypeTable = parentTypeTable;
        }

        private static ParseException NotASummandException => new ParseException("Not a summand");

        public static Either<ParseException, Pair<List<IToken>, SummandNode>> Parse(List<IToken> tokens, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Console.WriteLine("SummandNode");
            var maybePrimary = PrimaryNode.Parse(tokens, parentTypeTable);
            if (maybePrimary.IsRight)
            {
                tokens = maybePrimary.RightToList()[0].First;
                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;
                return new Pair<List<IToken>, SummandNode>(tokens, new SummandNode(maybePrimary.RightToList()[0].Second, parentTypeTable));
            }


            if (tokens.Count < 1)
                return NotASummandException;
            if (!(tokens[0] is LeftParenthSymbolToken))
                return NotASummandException;
            tokens = tokens.Skip(1).ToList();



            var maybeExpression = ExpressionNode.Parse(tokens, parentTypeTable);
            if (maybeExpression.IsLeft)
                return maybeExpression.LeftToList()[0];
            tokens = maybeExpression.RightToList()[0].First;



            if (tokens.Count < 1)
                return NotASummandException;
            if (!(tokens[0] is RightParenthSymbolToken))
                return NotASummandException;
            tokens = tokens.Skip(1).ToList();

            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                    tokens = tokens.Skip(1).ToList();
                else break;



            return new Pair<List<IToken>, SummandNode>(tokens, new SummandNode(maybeExpression.RightToList()[0].Second, parentTypeTable));
        }

    }
}