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
using LanguageExt.ClassInstances.Pred;

namespace ILangCompiler.Parser.AST
{
  public class ProgramNode : IAstNode
  {
    public List<IDeclarationNode> Declarations;
    public SymT SymbolTable;
    
    private ProgramNode(IEnumerable <IDeclarationNode> declarations, SymT symT)
    {
      Declarations = declarations.ToList();
      SymbolTable = symT;
    }
    

    public static Either<ParseException, ProgramNode> Parse(List<IToken> tokens)
    {
      Console.WriteLine("ProgramNode");
      
      var declarations = new List<IDeclarationNode>();
      var symT = new SymT();
      
      while (tokens.Count > 0)
      {
        var maybeRoutineDeclaration = RoutineDeclarationNode.Parse(tokens, symT);
        if (maybeRoutineDeclaration.IsRight)
        {
          tokens = maybeRoutineDeclaration.RightToList()[0].First;
          declarations.Add(maybeRoutineDeclaration.RightToList()[0].Second);
          continue;
        }

        var maybeVariableDeclaration = VariableDeclarationNode.Parse(tokens, symT);
        if (maybeVariableDeclaration.IsRight)
        {
          tokens = maybeVariableDeclaration.RightToList()[0].First;
          declarations.Add(maybeVariableDeclaration.RightToList()[0].Second);
          continue;
        }

        var maybeTypeDeclaration = TypeDeclarationNode.Parse(tokens, symT);
        if (maybeTypeDeclaration.IsRight)
        {
          tokens = maybeTypeDeclaration.RightToList()[0].First;
          declarations.Add(maybeTypeDeclaration.RightToList()[0].Second);
          continue;
        }

        return maybeTypeDeclaration.LeftToList()[0];
      }
      
      Console.WriteLine("Program is interprited Successfully");
      return new ProgramNode(declarations, symT);
      //return Either<ParseException, ProgramNode>.Bottom;
    }
  }
}