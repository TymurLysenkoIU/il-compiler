using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ILangCompiler.Parser.AST.Declarations
{
  public class RoutineDeclarationNode : IDeclarationNode
  {
    public IdentifierToken Identifier;
    public ImmutableArray<ParameterNode> Parameters;
    public Option<TypeNode> ReturnType;
    public BodyNode Body;

    private static ParseException NotARoutineException => new ParseException("Not a routine");

    private RoutineDeclarationNode(IdentifierToken identifier, IEnumerable<ParameterNode> parameters, Option<TypeNode> returnType, BodyNode body)
    {
      Identifier = identifier;
      Parameters = parameters.ToImmutableArray();
      ReturnType = returnType;
      Body = body;
    }

    private RoutineDeclarationNode()
    {
    }

    public static Either<ParseException, Pair<List<IToken>,RoutineDeclarationNode>> Parse(List<IToken> tokens)
    {
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
      while (true)
      {
        var maybeParameter = ParameterNode.Parse(tokens);

        if (maybeParameter.IsLeft)
        {
          break;
        }
        parameters.Add(maybeParameter.RightToList()[0].Second);
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
        maybeType = TypeNode.Parse(tokens);
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

      var maybeBody = BodyNode.Parse(tokens);
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
      
      return new Pair<List<IToken>, RoutineDeclarationNode>(tokens, new RoutineDeclarationNode(
        identifier, parameters, maybeType.Map<TypeNode>(pr => pr.Second).ToOption(),
        maybeBody.RightToList()[0].Second));
    
    }
  }
}