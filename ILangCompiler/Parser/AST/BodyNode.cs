using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.Parser.AST.Expressions;
using ILangCompiler.Parser.AST.Statements;
using ILangCompiler.Parser.AST.TypeTable;
using ILangCompiler.Parser.AST.TypeTable.TypeRepresentation;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.Constructions;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ILangCompiler.Parser.AST
{
    public class BodyNode : IAstNode, ITypeTable<IEntityType>
    {
        public ImmutableArray<IAstNode> Elements;
        public Option<ExpressionNode> ReturnExpressionNode = None;

        #region Type table

        private IDictionary<string, IEntityType> ScopeTypeTable;
        private readonly IScopedTable<IEntityType, string> ParentTypeTable;

        IDictionary<string, IEntityType> IScopedTable<IEntityType, string>.Table => ScopeTypeTable;

        Option<IScopedTable<IEntityType, string>> IScopedTable<IEntityType, string>.ParentTable => Some(ParentTypeTable);

        #endregion

        private BodyNode(ImmutableArray<IAstNode> elements, IDictionary<string, IEntityType> scopeTypeTable, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Elements = elements;
            ScopeTypeTable = scopeTypeTable;
            ParentTypeTable = parentTypeTable;
        }

        private BodyNode(ImmutableArray<IAstNode> elements, ExpressionNode returnExpressionNode, IDictionary<string, IEntityType> scopeTypeTable, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Elements = elements;
            ReturnExpressionNode = returnExpressionNode;
            ScopeTypeTable = scopeTypeTable;
            ParentTypeTable = parentTypeTable;
        }

        private static ParseException NotABodyException => new ParseException("Not a body");

        public static Either<ParseException, Pair<List<IToken>,BodyNode>> Parse(List<IToken> tokens, SymT symT, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Console.WriteLine("BodyNode");

            ImmutableArray<IAstNode> elements = ImmutableArray<IAstNode>.Empty;
            var typeTable = new Dictionary<string, IEntityType>();
            var result = new BodyNode(ImmutableArray<IAstNode>.Empty, typeTable, parentTypeTable);

            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                    tokens[0] is SemicolonSymbolToken)
                    tokens = tokens.Skip(1).ToList();
                else break;

            while (true)
            {
                var maybeSimpleDeclaration1 = VariableDeclarationNode.Parse(tokens, symT, result);
                if (maybeSimpleDeclaration1.IsRight)
                {
                    tokens = maybeSimpleDeclaration1.RightToList()[0].First;
                    var varDecl = maybeSimpleDeclaration1.RightToList()[0].Second;
                    elements = elements.Add(varDecl);
                    typeTable.TryAdd(varDecl.Identifier.Lexeme, varDecl.ToVariableType());
                    while (tokens.Count > 0)
                        if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                            tokens[0] is SemicolonSymbolToken)
                            tokens = tokens.Skip(1).ToList();
                        else break;
                    continue;
                }

                var maybeSimpleDeclaration2 = TypeDeclarationNode.Parse(tokens, symT, result);
                if (maybeSimpleDeclaration2.IsRight)
                {
                    tokens = maybeSimpleDeclaration2.RightToList()[0].First;
                    var typeDecl = maybeSimpleDeclaration2.RightToList()[0].Second;
                    elements = elements.Add(typeDecl);
                    typeTable.TryAdd(typeDecl.Identifier.Lexeme, typeDecl.ToTypeAliasType());
                    while (tokens.Count > 0)
                        if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                            tokens[0] is SemicolonSymbolToken)
                            tokens = tokens.Skip(1).ToList();
                        else break;
                    continue;
                }

                var maybeStatement = StatementNode.Parse(tokens, symT, result);
                if (maybeStatement.IsRight)
                {
                    tokens = maybeStatement.RightToList()[0].First;
                    elements = elements.Add(maybeStatement.RightToList()[0].Second);
                    while (tokens.Count > 0)
                        if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                            tokens[0] is SemicolonSymbolToken)
                            tokens = tokens.Skip(1).ToList();
                        else break;
                    continue;
                }

                var maybeExpression = StatementNode.Parse(tokens, symT, result);
                if (maybeExpression.IsRight)
                {
                    tokens = maybeExpression.RightToList()[0].First;
                    elements = elements.Add(maybeExpression.RightToList()[0].Second);
                    while (tokens.Count > 0)
                        if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                            tokens[0] is SemicolonSymbolToken)
                            tokens = tokens.Skip(1).ToList();
                        else break;
                    continue;
                }

                break;
            }


            if (tokens.Count < 1)
                return NotABodyException;
            if (!(tokens[0] is ReturnKeywordToken))
                return NotABodyException;
            tokens = tokens.Skip(1).ToList();
            var maybeExpression1 = ExpressionNode.Parse(tokens, result);
            if (maybeExpression1.IsRight)
            {

                while (tokens.Count > 0)
                    if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                        tokens[0] is SemicolonSymbolToken)
                        tokens = tokens.Skip(1).ToList();
                    else break;
                tokens = maybeExpression1.RightToList()[0].First;

                result.Elements = elements;
                result.ReturnExpressionNode = maybeExpression1.RightToList()[0].Second;
                var res = new Pair<List<IToken>,BodyNode>(tokens, result);

                return res;
            }



            result.Elements = elements;
            return new Pair<List<IToken>,BodyNode>(tokens, result);
        }

    }
}