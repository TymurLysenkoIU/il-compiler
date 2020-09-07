using System.Collections.Generic;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using LanguageExt;

namespace ILangCompiler.Parser.AST
{
  public interface IAstNode
  {
    // Either<ParseException, T> Parse(List<IToken> tokens);
  }
}