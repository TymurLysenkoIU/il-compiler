using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.AST.TypeTable;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ILangCompiler.Parser.AST.Declarations.Types
{
    public class TypeDeclarationNode: ISimpleDeclarationNode, ITypeTable<IEntityType>
    {
        public IdentifierToken Identifier;
        public TypeNode Type;

        #region Type table

        private IDictionary<string, IEntityType> ScopeTypeTable = new Dictionary<string, IEntityType>();
        private readonly IScopedTable<IEntityType, string> ParentTypeTable;

        IDictionary<string, IEntityType> IScopedTable<IEntityType, string>.Table => ScopeTypeTable;

        Option<IScopedTable<IEntityType, string>> IScopedTable<IEntityType, string>.ParentTable => Some(ParentTypeTable);

        #endregion

        private TypeDeclarationNode(IdentifierToken identifier, TypeNode type, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Identifier = identifier;
            Type = type;
            ParentTypeTable = parentTypeTable;
        }

        private static ParseException NotATypeDeclarationException => new ParseException("Not a type declaration");

        public static Either<ParseException, Pair<List<IToken>,TypeDeclarationNode>> Parse(List<IToken> tokens, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Console.WriteLine("TypeDeclarationNode");
            if (tokens.Count < 3)
                return NotATypeDeclarationException;
            if (!(tokens[0] is TypeKeywordToken) ||
                !(tokens[1] is IdentifierToken) ||
                !(tokens[2] is IsKeywordToken))
                return NotATypeDeclarationException;


            IdentifierToken identifier = (IdentifierToken) tokens[1];
            tokens = tokens.Skip(3).ToList();

            var maybeType = TypeNode.Parse(tokens, parentTypeTable);
            if (maybeType.IsLeft)
                return maybeType.LeftToList()[0];
            tokens = maybeType.RightToList()[0].First;
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                    tokens[0] is SemicolonSymbolToken)
                    tokens = tokens.Skip(1).ToList();
                else break;

            return new Pair<List<IToken>, TypeDeclarationNode>(tokens, new TypeDeclarationNode(
                identifier, maybeType.RightToList()[0].Second, parentTypeTable));
        }

    }
}