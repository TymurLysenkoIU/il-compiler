using System.Collections.Immutable;
using System.Linq;
using ILangCompiler.AST;
using ILangCompiler.Parser.Abstractions;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined;
using LanguageExt;

namespace ILangCompiler.Parser.Combinators
{
  public class ConstantParser<TR, TT> : IParser<ParseException, TR>
    where TR : new()
    where TT : PredefinedToken
  {
    public Either<ParseException, ParseResult<TR>> TryParse(ImmutableList<IToken> tokens) =>
      tokens.HeadOrNone().ToEither<ParseException>(() => new EmptySequenceException())
        .Match<Either<ParseException, ParseResult<TR>>>(
          Left: e => e,
          Right: t =>
          {
            if (t is TT)
              return new ParseResult<TR>(new TR(), tokens.Skip(1).ToImmutableList());
            else
              return new ParseException("");
          });
  }
}