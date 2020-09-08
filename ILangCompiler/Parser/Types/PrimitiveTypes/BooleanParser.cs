using System.Collections.Immutable;
using System.Linq;
using ILangCompiler.AST.Types.PrimitiveTypes;
using ILangCompiler.Parser.Abstractions;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.PrimitiveTypes;
using LanguageExt;

namespace ILangCompiler.Parser.Types.PrimitiveTypes
{
  public class BooleanParser : IParser<ParseException, IPrimitiveTypeNode>, IParser<ParseException, BooleanNode>
    // : CastingPrimitiveTypeParser<ParseException, BooleanNode>
  {
    public Either<ParseException, ParseResult<BooleanNode>> TryParse(ImmutableList<IToken> tokens) =>
      tokens.HeadOrNone().ToEither<ParseException>(() => new EmptySequenceException())
        .Match<Either<ParseException, ParseResult<BooleanNode>>>(
          Left: e => e,
          Right: t =>
          {
            if (t is BooleanKeywordToken)
              return new ParseResult<BooleanNode>(new BooleanNode(), tokens.Skip(1).ToImmutableList());
            else
              return new ParseException("");
          });

    Either<ParseException, ParseResult<IPrimitiveTypeNode>> IParser<ParseException, IPrimitiveTypeNode>.TryParse(
      ImmutableList<IToken> tokens
    ) =>
      TryParse(tokens).Map(pr => new ParseResult<IPrimitiveTypeNode>(
        pr.Result,
        pr.RestTokens
      ));
  }
}