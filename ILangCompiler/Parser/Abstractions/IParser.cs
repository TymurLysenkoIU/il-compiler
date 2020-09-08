using System.Collections.Generic;
using System.Collections.Immutable;
using ILangCompiler.AST;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using LanguageExt;

namespace ILangCompiler.Parser.Abstractions
{
  public interface IParser<TE, TR>
    where TE : ParseException
    where TR : IAstNode
  {
    Either<TE, ParseResult<TR>> TryParse(ImmutableList<IToken> tokens);
  }
}