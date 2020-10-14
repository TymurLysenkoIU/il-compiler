using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.AST.Expressions;
using ILangCompiler.Parser.AST.TypeTable;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ILangCompiler.Parser.AST.Declarations.Types
{
    public class VariableDeclarationNode: ISimpleDeclarationNode, ITypeTable<IEntityType>
    {
        public IdentifierToken Identifier;
        public Option<TypeNode> Type;
        public Option<ExpressionNode> Expression;

        #region Type table

        private IDictionary<string, IEntityType> ScopeTypeTable = new Dictionary<string, IEntityType>();
        private readonly IScopedTable<IEntityType, string> ParentTypeTable;

        IDictionary<string, IEntityType> IScopedTable<IEntityType, string>.Table => ScopeTypeTable;

        Option<IScopedTable<IEntityType, string>> IScopedTable<IEntityType, string>.ParentTable => Some(ParentTypeTable);

        #endregion

        private static ParseException NotAVariableDeclarationException => new ParseException("Not a variable declaration");

        private VariableDeclarationNode(IdentifierToken identifier, Option<TypeNode> type, Option<ExpressionNode> expression, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Identifier = identifier;
            Type = type;
            Expression = expression;
            ParentTypeTable = parentTypeTable;
        }

        public static Either<ParseException, Pair<List<IToken>, VariableDeclarationNode>> Parse(List<IToken> tokens, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Console.WriteLine("VariableDeclarationNode");
            if (tokens.Count < 3)
                return NotAVariableDeclarationException;
            var maybeVar = tokens[0];
            var maybeIdentifier = tokens[1];
            var maybeColonOrIs = tokens[2];
            if (
                !(maybeVar is VarKeywordToken) ||
                !(maybeIdentifier is IdentifierToken) ||
                !(maybeColonOrIs is ColonSymbolToken || maybeColonOrIs is IsKeywordToken)
            )
                return NotAVariableDeclarationException;



            IdentifierToken identifier = (IdentifierToken) maybeIdentifier;
            tokens = tokens.Skip(2).ToList();

            if (tokens[0] is IsKeywordToken)
            {
                tokens = tokens.Skip(1).ToList();

                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;


                var maybeExpression = ExpressionNode.Parse(tokens, parentTypeTable);

                if (maybeExpression.IsLeft)
                    return maybeExpression.LeftToList()[0];
                tokens = maybeExpression.RightToList()[0].First;

                return new Pair<List<IToken>, VariableDeclarationNode>(tokens, new VariableDeclarationNode(
                    identifier, Option<TypeNode>.None, maybeExpression.RightToList()[0].Second, parentTypeTable));
                //return new VariableDeclarationNode(identifier, Option<TypeNode>.None, maybeExpression);
            }

            else if (tokens[0] is ColonSymbolToken)
            {
                tokens = tokens.Skip(1).ToList();

                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;

                var maybeType = TypeNode.Parse(tokens, parentTypeTable);
                if (maybeType.IsLeft)
                    return maybeType.LeftToList()[0];
                tokens = maybeType.RightToList()[0].First;
                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;

                if (!(tokens[0] is IsKeywordToken))
                    return new Pair<List<IToken>, VariableDeclarationNode>(tokens, new VariableDeclarationNode(
                        identifier, maybeType.RightToList()[0].Second, Option<ExpressionNode>.None, parentTypeTable));


                tokens = tokens.Skip(1).ToList();

                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;


                var maybeExpression = ExpressionNode.Parse(tokens, parentTypeTable);

                if (maybeExpression.IsLeft)
                    return maybeExpression.LeftToList()[0];
                tokens = maybeExpression.RightToList()[0].First;
                return new Pair<List<IToken>, VariableDeclarationNode>(tokens, new VariableDeclarationNode(
                    identifier, maybeType.RightToList()[0].Second, maybeExpression.RightToList()[0].Second, parentTypeTable));
            }

            return NotAVariableDeclarationException;
        }
    }
}