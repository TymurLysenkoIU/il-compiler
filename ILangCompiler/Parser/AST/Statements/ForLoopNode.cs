using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.AST.Declarations.Types.PrimitiveTypes;
using ILangCompiler.Parser.AST.TypeTable;
using ILangCompiler.Parser.AST.TypeTable.TypeRepresentation;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.Constructions;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ILangCompiler.Parser.AST.Statements
{
    public class ForLoopNode:IAstNode, ITypeTable<IEntityType>
    {
        public IdentifierToken Identifier;
        public RangeNode Range;
        public BodyNode Body;

        #region Type table

        private IDictionary<string, IEntityType> ScopeTypeTable;
        private readonly IScopedTable<IEntityType, string> ParentTypeTable;

        IDictionary<string, IEntityType> IScopedTable<IEntityType, string>.Table => ScopeTypeTable;

        Option<IScopedTable<IEntityType, string>> IScopedTable<IEntityType, string>.ParentTable => Some(ParentTypeTable);

        #endregion

        public SymT SymbolTable;

        private ForLoopNode(IdentifierToken identifier, RangeNode range, BodyNode body, SymT symT, IDictionary<string, IEntityType> scopeTypeTable, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Identifier = identifier;
            Range = range;
            Body = body;
            SymbolTable = symT;
            ScopeTypeTable = scopeTypeTable;
            ParentTypeTable = parentTypeTable;
        }

        private static ParseException NotAForLoopException => new ParseException("Not a for loop");

        public static Either<ParseException, Pair<List<IToken>,ForLoopNode>> Parse(List<IToken> tokens, SymT symT, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Console.WriteLine("ForLoopNode");
            SymT NewSymT = new SymT(symT);

            if (tokens.Count < 1)
                return NotAForLoopException;
            if (!(tokens[0] is ForKeywordToken))
                return NotAForLoopException;
            tokens = tokens.Skip(1).ToList();

            if (tokens.Count < 1)
                return NotAForLoopException;
            if (!(tokens[0] is IdentifierToken))
                return NotAForLoopException;
            IdentifierToken identifier = (IdentifierToken) tokens[0];
            var typeTable = new Dictionary<string, IEntityType>();
            typeTable.TryAdd(
                identifier.Lexeme,
                new VariableType(new PrimitiveTypeRepresentation(new IntegerTypeNode()))
            );
            tokens = tokens.Skip(1).ToList();

            var result = new ForLoopNode(
                identifier, null, null, NewSymT, typeTable, parentTypeTable);
            var maybeRange = RangeNode.Parse(tokens, result);

            if (NewSymT.Contain(identifier))
            {
                // TODO: return an exception instead
                Console.WriteLine("Repeating identifier in the same scope");
            }
            else
            {
                NewSymT.Add(identifier);
            }

            if (maybeRange.IsLeft)
                return maybeRange.LeftToList()[0];
            RangeNode range = maybeRange.RightToList()[0].Second;
            tokens = maybeRange.RightToList()[0].First;

            if (tokens.Count < 1)
                return NotAForLoopException;
            if (!(tokens[0] is LoopKeywordToken))
                return NotAForLoopException;
            tokens = tokens.Skip(1).ToList();

            var maybeBody = BodyNode.Parse(tokens, NewSymT, result);
            if (maybeBody.IsLeft)
                return maybeBody.LeftToList()[0];
            BodyNode body = maybeBody.RightToList()[0].Second;
            tokens = maybeBody.RightToList()[0].First;

            if (tokens.Count < 1)
                return NotAForLoopException;
            if (!(tokens[0] is EndKeywordToken))
                return NotAForLoopException;
            tokens = tokens.Skip(1).ToList();

            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                    tokens[0] is SemicolonSymbolToken)
                    tokens = tokens.Skip(1).ToList();
                else break;

            result.Range = range;
            result.Body = body;
            return new Pair<List<IToken>, ForLoopNode>(tokens, result);
        }
    }
}