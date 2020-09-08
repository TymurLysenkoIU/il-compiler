using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Declarations
{
  public class RoutineDeclarationNode : IDeclarationNode
  {
    public IdentifierToken Identifier { get; }
    public ImmutableArray<ParameterNode> Parameters { get; }
    public Option<ParameterNode> ReturnType { get; }
    public BodyNode Body { get; }

    private static ParseException NotARoutineException => new ParseException("Not a routine");

    private RoutineDeclarationNode(IdentifierToken identifier, IEnumerable<ParameterNode> parameters, Option<ParameterNode> returnType, BodyNode body)
    {
      Identifier = identifier;
      Parameters = parameters.ToImmutableArray();
      ReturnType = returnType;
      Body = body;
    }

    public static Either<ParseException, RoutineDeclarationNode> Parse(List<IToken> tokens)
    {
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
      while (!(tokens[0] is RightParenthSymbolToken))
      {
        var maybeParameter = ParameterNode.Parse(tokens);

        if (maybeParameter.IsLeft)
        {
          return maybeParameter.LeftToList()[0];
        }

        maybeParameter.Do(p => parameters.Add(p));

        if (tokens.Count > 0)
        {
          if (!(tokens[0] is ComaSymbolToken))
            return NotARoutineException;

          tokens = tokens.Skip(1).ToList();
        }
        else
        {
          return NotARoutineException;
        }
      }
      tokens = tokens.Skip(1).ToList();

      //Option<ParameterNode> returnType = 
      // TODO: parse type, if the next token is :
      // TODO: parse is keyword
      // TODO: parse BodyNode

      //Option<ParameterNode> returnType = ;
      //Option<BodyNode> body = ;

      // TODO: parse end in the end

      // return new RoutineDeclarationNode(identifier, parameters, );
      //return new RoutineDeclarationNode(identifier, parameters, returnType, body);
      return new ParseException("Simple declaration is not implemented");
    }
  }
}