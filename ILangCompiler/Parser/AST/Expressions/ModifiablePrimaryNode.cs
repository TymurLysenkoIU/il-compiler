using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.Parser.AST.TypeTable;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ILangCompiler.Parser.AST.Expressions
{
    public class ModifiablePrimaryNode: IAstNode, ITypeTable<IEntityType>
    {
        private IdentifierToken Identifier;
        private List<IdentifierToken> Identifiers;
        private List<ExpressionNode> Expressions;
        //TODO transform these two lists into one "Either" list

        #region Type table

        private IDictionary<string, IEntityType> ScopeTypeTable = new Dictionary<string, IEntityType>();
        private readonly IScopedTable<IEntityType, string> ParentTypeTable;

        IDictionary<string, IEntityType> IScopedTable<IEntityType, string>.Table => ScopeTypeTable;

        Option<IScopedTable<IEntityType, string>> IScopedTable<IEntityType, string>.ParentTable => Some(ParentTypeTable);

        #endregion

        private ModifiablePrimaryNode(IdentifierToken identifier, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Identifier = identifier;
            ParentTypeTable = parentTypeTable;
        }
        private ModifiablePrimaryNode(IdentifierToken identifier, List<IdentifierToken> identifiers, List<ExpressionNode> expressions, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Identifier = identifier;
            Identifiers = identifiers;
            Expressions = expressions;
            ParentTypeTable = parentTypeTable;
        }

        private static ParseException NotAModifiablePrimaryException => new ParseException("Not a modifiable primary");

        public static Either<ParseException, Pair<List<IToken>,ModifiablePrimaryNode>> Parse(List<IToken> tokens, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Console.WriteLine("ModifiablePrimaryNode");

            if (tokens.Count < 1)
                return NotAModifiablePrimaryException;
            if (!(tokens[0] is IdentifierToken))
                return NotAModifiablePrimaryException;
            IdentifierToken identifier = (IdentifierToken)tokens[0];
            tokens = tokens.Skip(1).ToList();
            var identifiers = new List<IdentifierToken>();
            var expressions = new List<ExpressionNode>();
            while (true)
            {
                if (tokens.Count < 1)
                    return new Pair<List<IToken>,ModifiablePrimaryNode> (tokens, new ModifiablePrimaryNode(identifier, parentTypeTable));

                if (tokens[0] is DotSymbolToken)
                {
                    tokens = tokens.Skip(1).ToList();
                    if (tokens.Count < 1)
                        return NotAModifiablePrimaryException;
                    if (!(tokens[0] is IdentifierToken))
                        return NotAModifiablePrimaryException;
                    identifiers.Add((IdentifierToken)tokens[0]);
                    tokens = tokens.Skip(1).ToList();
                }

                else if (tokens[0] is LeftBracketSymbolToken)
                {
                    tokens = tokens.Skip(1).ToList();
                    var maybeExpression = ExpressionNode.Parse(tokens, parentTypeTable);
                    if (maybeExpression.IsLeft)
                        return maybeExpression.LeftToList()[0];
                    tokens = maybeExpression.RightToList()[0].First;
                    if (tokens.Count < 1)
                        return NotAModifiablePrimaryException;
                    if (!(tokens[0] is RightBracketSymbolToken))
                        return NotAModifiablePrimaryException;
                    expressions.Add(maybeExpression.RightToList()[0].Second);
                    tokens = tokens.Skip(1).ToList();

                }
                else break;

            }

            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                    tokens[0] is SemicolonSymbolToken)
                    tokens = tokens.Skip(1).ToList();
                else break;
            return new Pair<List<IToken>,ModifiablePrimaryNode> (tokens, new ModifiablePrimaryNode(identifier,identifiers,expressions, parentTypeTable));;
        }

    }
}