using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using ILangCompiler.Parser.AST.Declarations;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration;
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
      Console.WriteLine("ProgramNode");
      
      var declarations = new List<IDeclarationNode>();

      
      while (tokens.Count > 0)
      {
        var maybeRoutineDeclaration = RoutineDeclarationNode.Parse(tokens);
        if (maybeRoutineDeclaration.IsRight)
          continue;
        var maybeVariableDeclaration = VariableDeclarationNode.Parse(tokens);
        if (maybeVariableDeclaration.IsRight)
          continue;
        var maybeTypeDeclaration = TypeDeclarationNode.Parse(tokens);
        if (maybeTypeDeclaration.IsRight)
          continue;
        return maybeTypeDeclaration.LeftToList()[0];
      }

      return Either<ParseException, ProgramNode>.Bottom;
    }
  }
}