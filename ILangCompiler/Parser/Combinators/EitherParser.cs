using System.Collections.Immutable;
using System.Threading.Tasks;
using ILangCompiler.AST;
using ILangCompiler.Parser.Abstractions;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using LanguageExt;

namespace ILangCompiler.Parser.Combinators
{
  public abstract class EitherParser<TE, TR> : IParser<TE, TR>
    where TE : ParseException, new()
    where TR : IAstNode
  {
    protected ImmutableArray<IParser<TE, TR>> Parsers;

    public EitherParser(ImmutableArray<IParser<TE, TR>> parsers)
    {
      Parsers = parsers;
    }

    public Either<TE, ParseResult<TR>> TryParse(ImmutableList<IToken> tokens)
    {
      Either<TE, ParseResult<TR>> result = new TE();

      var parallel = Parallel.ForEach(Parsers, (parser, state) =>
      {
        var parseResult = parser.TryParse(tokens);

        parseResult.Match(
          Left: _ => { },
          Right: pr =>
          {
            state.Break();
            result = pr;
          }
        );
      });

      return result;
    }
  }

  public class EitherParser<
    TE1,
    TR1,
    TE2,
    TR2,
    TE,
    TR
  > : EitherParser<TE, TR>
    where TE1 : TE
    where TR1 : TR

    where TE2 : TE
    where TR2 : TR

    where TE : ParseException, new()
    where TR : IAstNode
  {
    private ImmutableArray<IParser<TE, TR>> Parsers;

    public EitherParser(
      IParser<TE1, TR1> parser1,
      IParser<TE2, TR2> parser2
    ) : base(new[]
      {
        (IParser<TE, TR>) parser1,
        (IParser<TE, TR>) parser2,
      }.ToImmutableArray()
    ) { }
  }

  public class EitherParser<
    TE1,
    TR1,
    TE2,
    TR2,
    TE3,
    TR3,
    TE,
    TR
  > : EitherParser<TE, TR>
    where TE1 : TE
    where TR1 : TR

    where TE2 : TE
    where TR2 : TR

    where TE3 : TE
    where TR3 : TR

    where TE : ParseException, new()
    where TR : IAstNode
  {
    private ImmutableArray<IParser<TE, TR>> Parsers;

    public EitherParser(
      IParser<TE1, TR1> parser1,
      IParser<TE2, TR2> parser2,
      IParser<TE3, TR3> parser3
    ) : base(new[]
      {
        (IParser<TE, TR>) parser1,
        (IParser<TE, TR>) parser2,
        (IParser<TE, TR>) parser3,
      }.ToImmutableArray()
    ) { }
  }

  public class EitherParser<
    TE1,
    TR1,
    TE2,
    TR2,
    TE3,
    TR3,
    TE4,
    TR4,
    TE,
    TR
  > : EitherParser<TE, TR>
    where TE1 : TE
    where TR1 : TR

    where TE2 : TE
    where TR2 : TR

    where TE3 : TE
    where TR3 : TR

    where TE4 : TE
    where TR4 : TR

    where TE : ParseException, new()
    where TR : IAstNode
  {
    private ImmutableArray<IParser<TE, TR>> Parsers;

    public EitherParser(
      IParser<TE1, TR1> parser1,
      IParser<TE2, TR2> parser2,
      IParser<TE3, TR3> parser3,
      IParser<TE4, TR4> parser4
    ) : base(new[]
      {
        (IParser<TE, TR>) parser1,
        (IParser<TE, TR>) parser2,
        (IParser<TE, TR>) parser3,
        (IParser<TE, TR>) parser4,
      }.ToImmutableArray()
    ) { }
  }

  public class EitherParser<
    TE1,
    TR1,
    TE2,
    TR2,
    TE3,
    TR3,
    TE4,
    TR4,
    TE5,
    TR5,
    TE,
    TR
  > : EitherParser<TE, TR>
    where TE1 : TE
    where TR1 : TR

    where TE2 : TE
    where TR2 : TR

    where TE3 : TE
    where TR3 : TR

    where TE4 : TE
    where TR4 : TR

    where TE5 : TE
    where TR5 : TR

    where TE : ParseException, new()
    where TR : IAstNode
  {
    private ImmutableArray<IParser<TE, TR>> Parsers;

    public EitherParser(
      IParser<TE1, TR1> parser1,
      IParser<TE2, TR2> parser2,
      IParser<TE3, TR3> parser3,
      IParser<TE4, TR4> parser4,
      IParser<TE5, TR5> parser5
    ) : base(new[]
      {
        (IParser<TE, TR>) parser1,
        (IParser<TE, TR>) parser2,
        (IParser<TE, TR>) parser3,
        (IParser<TE, TR>) parser4,
        (IParser<TE, TR>) parser5,
      }.ToImmutableArray()
    ) { }
  }
}