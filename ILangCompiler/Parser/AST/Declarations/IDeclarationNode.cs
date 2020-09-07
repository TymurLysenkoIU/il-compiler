using System.Collections.Generic;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Declarations
{
  public interface IDeclarationNode : IAstNode
  {
      public static Either<ParseException, IDeclarationNode> Parse(List<IToken> tokens)
      {
          var maybeRoutineDeclaration = RoutineDeclarationNode.Parse(tokens);
          var maybeSimpleDeclaration = ISimpleDeclarationNode.Parse(tokens);

          return maybeRoutineDeclaration.Match(
              Left: _ => maybeSimpleDeclaration.BiMap(
                    Left: _ => new ParseException("Error parsing declaration"),
                    Right: sd => (IDeclarationNode) sd
                  ),
              Right: rd => (Either<ParseException, IDeclarationNode>) rd
          );
      }
  }
}