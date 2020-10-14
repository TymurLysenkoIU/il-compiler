using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.Parser.AST.TypeTable;
using ILangCompiler.Parser.AST.TypeTable.TypeRepresentation;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ILangCompiler.Parser.AST.Declarations
{
  public class RoutineDeclarationNode : IDeclarationNode, ITypeTable<IEntityType>
  {
    public IdentifierToken Identifier;
    public ImmutableArray<ParameterNode> Parameters;
    public Option<TypeNode> ReturnType;
    public BodyNode Body;
    public SymT SymbolTable;

    #region Type table

    private readonly IDictionary<string, IEntityType> ScopeTypeTable;
    private readonly IScopedTable<IEntityType, string> ParentTypeTable;

    IDictionary<string, IEntityType> IScopedTable<IEntityType, string>.Table => ScopeTypeTable;

    Option<IScopedTable<IEntityType, string>> IScopedTable<IEntityType, string>.ParentTable => Some(ParentTypeTable);

    #endregion


    private static ParseException NotARoutineException => new ParseException("Not a routine");

    private RoutineDeclarationNode(IdentifierToken identifier, IEnumerable<ParameterNode> parameters, Option<TypeNode> returnType, BodyNode body, SymT symT, IDictionary<string, IEntityType> scopeTypeTable, IScopedTable<IEntityType, string> parentTypeTable)
    {
      Identifier = identifier;
      Parameters = parameters.ToImmutableArray();
      ReturnType = returnType;
      Body = body;
      ScopeTypeTable = scopeTypeTable;
      ParentTypeTable = parentTypeTable;
      SymbolTable = symT;
    }

    private RoutineDeclarationNode(IDictionary<string, IEntityType> scopeTypeTable, IScopedTable<IEntityType, string> parentTypeTable)
    {
      ScopeTypeTable = scopeTypeTable;
      ParentTypeTable = parentTypeTable;
    }

    public static Either<ParseException, Pair<List<IToken>,RoutineDeclarationNode>> Parse(List<IToken> tokens, SymT symT, IScopedTable<IEntityType, string> parentTypeTable)
    {
      var NewSymT = new SymT(symT);

      Console.WriteLine("RoutineDeclarationNode");
      if (tokens.Count <= 3)
        return NotARoutineException;

      var maybeRoutine = tokens[0];
      var maybeIdentifier = tokens[1];
      var maybeLp = tokens[2];

      if (
        !(maybeRoutine is RoutineKeywordToken) ||
        !(maybeIdentifier is IdentifierToken) ||
        !(maybeLp is LeftParenthSymbolToken)
      )
        return NotARoutineException;

      IdentifierToken identifier = (IdentifierToken) maybeIdentifier;

      tokens = tokens.Skip(3).ToList();

      var parameters = new List<ParameterNode>();
      var typeTable = new Dictionary<string, IEntityType>();

      while (true)
      {
        var maybeParameter = ParameterNode.Parse(tokens, NewSymT, parentTypeTable);

        if (maybeParameter.IsLeft)
        {
          break;
        }

        var parameter = maybeParameter.RightToList()[0].Second;
        parameters.Add(parameter);
        typeTable.Add(parameter.Name.Lexeme, new VariableType(parameter.Type.ToTypeRepresentation()));
        tokens = maybeParameter.RightToList()[0].First;

        if (tokens.Count < 1)
          return NotARoutineException;

        if (tokens[0] is ComaSymbolToken)
        {
          tokens = tokens.Skip(1).ToList();
          continue;
        }

        break;
      }

      if (tokens[0] is RightParenthSymbolToken)
      {
        tokens = tokens.Skip(1).ToList();
      }
      else
      {
        return NotARoutineException;
      }

      if (tokens.Count < 2)
        return NotARoutineException;

      Either<ParseException, Pair<List<IToken>, TypeNode>> maybeType = new ParseException("Dummy");
      if (tokens[0] is ColonSymbolToken)
      {
        tokens = tokens.Skip(1).ToList();
        maybeType = TypeNode.Parse(tokens, symT, parentTypeTable);
        if (maybeType.IsLeft)
          return NotARoutineException;
        tokens = maybeType.RightToList()[0].First;
      }

      if (tokens.Count < 1)
        return NotARoutineException;
      if (!(tokens[0] is IsKeywordToken))
      {
        return NotARoutineException;
      }

      tokens = tokens.Skip(1).ToList();

      var result = new RoutineDeclarationNode(
        identifier, parameters,
        maybeType.Map<TypeNode>(pr => pr.Second).ToOption(),
        null,
        NewSymT,
        typeTable,
        parentTypeTable
      );
      var maybeBody = BodyNode.Parse(tokens, NewSymT, result);
      if (maybeBody.IsLeft)
      {
        return maybeBody.LeftToList()[0];
      }

      tokens = maybeBody.RightToList()[0].First;

      if (tokens.Count < 1)
        return NotARoutineException;
      if (!(tokens[0] is EndKeywordToken))
      {
        return NotARoutineException;
      }


      tokens = tokens.Skip(1).ToList();

      while (tokens.Count > 0)
        if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken||
            tokens[0] is SemicolonSymbolToken)
          tokens = tokens.Skip(1).ToList();
        else break;

      result.Body = maybeBody.RightToList()[0].Second;
      return new Pair<List<IToken>, RoutineDeclarationNode>(tokens, result);
    }
  }
}