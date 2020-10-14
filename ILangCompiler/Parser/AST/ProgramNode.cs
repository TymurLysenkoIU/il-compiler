using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using ILangCompiler.Parser.AST.Declarations;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.Parser.AST.TypeTable;
using ILangCompiler.Parser.AST.TypeTable.TypeRepresentation;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ILangCompiler.Parser.AST
{
  public class ProgramNode : IAstNode, ITypeTable<IEntityType>
  {
    public List<IDeclarationNode> Declarations;

    #region Type table

    private readonly IDictionary<string, IEntityType> ScopeTypeTable;

    IDictionary<string, IEntityType> IScopedTable<IEntityType, string>.Table => ScopeTypeTable;

    Option<IScopedTable<IEntityType, string>> IScopedTable<IEntityType, string>.ParentTable { get; } = None;

    #endregion

    public SymT SymbolTable;

    private ProgramNode(
      IEnumerable<IDeclarationNode> declarations,
      SymT symT,
      IDictionary<string, IEntityType> scopeTypeTable
    )
    {
      Declarations = declarations.ToList();
      SymbolTable = symT;
      ScopeTypeTable = scopeTypeTable;
    }


    public static Either<ParseException, ProgramNode> Parse(List<IToken> tokens)
    {
      Console.WriteLine("ProgramNode");

      var declarations = new List<IDeclarationNode>();
      var typeTable = new Dictionary<string, IEntityType>();
      var symT = new SymT();

      var result = new ProgramNode(declarations, symT, typeTable);

      while (tokens.Count > 0)
      {
        var maybeRoutineDeclaration = RoutineDeclarationNode.Parse(tokens, symT, result);
        if (maybeRoutineDeclaration.IsRight)
        {
          tokens = maybeRoutineDeclaration.RightToList()[0].First;

          var routineDeclaration = maybeRoutineDeclaration.RightToList()[0].Second;
          declarations.Add(routineDeclaration);
          typeTable.Add(routineDeclaration.Identifier.Lexeme, routineDeclaration.ToRoutineType());
          continue;
        }

        var maybeVariableDeclaration = VariableDeclarationNode.Parse(tokens, symT, result);
        if (maybeVariableDeclaration.IsRight)
        {
          tokens = maybeVariableDeclaration.RightToList()[0].First;
          var varDecl = maybeVariableDeclaration.RightToList()[0].Second;
          declarations.Add(varDecl);
          typeTable.Add(varDecl.Identifier.Lexeme, varDecl.ToVariableType());
          continue;
        }

        var maybeTypeDeclaration = TypeDeclarationNode.Parse(tokens, symT, result);
        if (maybeTypeDeclaration.IsRight)
        {
          tokens = maybeTypeDeclaration.RightToList()[0].First;
          var typeDecl = maybeTypeDeclaration.RightToList()[0].Second;
          declarations.Add(typeDecl);
          typeTable.Add(typeDecl.Identifier.Lexeme, typeDecl.ToTypeAliasType());
          continue;
        }

        return maybeTypeDeclaration.LeftToList()[0];
      }

      Console.WriteLine("Program is interprited Successfully");
      return new ProgramNode(declarations, symT, typeTable);
      //return Either<ParseException, ProgramNode>.Bottom;
    }
  }
}
