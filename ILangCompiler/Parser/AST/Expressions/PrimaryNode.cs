using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.Parser.AST.TypeTable;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Literals;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ILangCompiler.Parser.AST.Expressions
{
    public class PrimaryNode : IAstNode, ITypeTable<IEntityType>
    {
        private IToken Token;
        private ModifiablePrimaryNode ModifiablePrimary;

        #region Type table

        private IDictionary<string, IEntityType> ScopeTypeTable = new Dictionary<string, IEntityType>();
        private readonly IScopedTable<IEntityType, string> ParentTypeTable;

        IDictionary<string, IEntityType> IScopedTable<IEntityType, string>.Table => ScopeTypeTable;

        Option<IScopedTable<IEntityType, string>> IScopedTable<IEntityType, string>.ParentTable => Some(ParentTypeTable);

        #endregion

        private PrimaryNode(IToken token, ModifiablePrimaryNode modifiablePrimary, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Token = token;
            ModifiablePrimary = modifiablePrimary;
            ParentTypeTable = parentTypeTable;
        }

        private PrimaryNode(IToken token, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Token = token;
            ParentTypeTable = parentTypeTable;
        }

        private static ParseException NotAPrimaryException => new ParseException("Not a primary");

        public static Either<ParseException, Pair<List<IToken>, PrimaryNode>> Parse(List<IToken> tokens, IScopedTable<IEntityType, string> parentTypeTable)
        {
            IToken token = null;
            Console.WriteLine("PrimaryNode");
            if (tokens.Count < 1)
                return NotAPrimaryException;
            if ((tokens[0] is IntegerLiteralToken) || (tokens[0] is RealLiteralToken) ||
                (tokens[0] is TrueKeywordToken) || (tokens[0] is FalseKeywordToken))
            {
                token = tokens[0];
                tokens = tokens.Skip(1).ToList();
                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;
                return new Pair<List<IToken>, PrimaryNode>(tokens, new PrimaryNode(token, parentTypeTable));
            }

            var maybeModifiablePrimary = ModifiablePrimaryNode.Parse(tokens, parentTypeTable);
            if (maybeModifiablePrimary.IsLeft)
                return maybeModifiablePrimary.LeftToList()[0];
            var modifiablePrimary = maybeModifiablePrimary.RightToList()[0].Second;
            tokens = maybeModifiablePrimary.RightToList()[0].First;
            return new Pair<List<IToken>, PrimaryNode>(tokens, new PrimaryNode(token, modifiablePrimary, parentTypeTable));
        }
    }
}