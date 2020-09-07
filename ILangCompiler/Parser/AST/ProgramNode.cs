using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.AST.Declarations;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using LanguageExt;

namespace ILangCompiler.Parser.AST
{
  public class ProgramNode : IAstNode
  {
    public List<IDeclarationNode> Declarations;

    private ProgramNode(IEnumerable<IDeclarationNode> declarations)
    {
      Declarations = declarations.ToList();
    }

    public static Either<ParseException, ProgramNode> Parse(List<IToken> tokens)
    {
      var declarations = new List<IDeclarationNode>();

      while (tokens.Any())
      {
        var maybeDeclaration = IDeclarationNode.Parse(tokens);

        // TODO: maybe can be written better
        if (maybeDeclaration.IsLeft)
        {
          return maybeDeclaration.LeftToList()[0];
          // return maybeDeclaration.Map(_ => (ProgramNode) null);
        }

        maybeDeclaration.Do(d => declarations.Add(d));
      }

      return Either<ParseException, ProgramNode>.Bottom;
    }
  }
}