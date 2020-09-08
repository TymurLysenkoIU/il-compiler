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
  public class RealParser : IParser<ParseException, IPrimitiveTypeNode>, IParser<ParseException, RealNode> //: CastingPrimitiveTypeParser<ParseException, RealNode>
  {
    public Either<ParseException, ParseResult<RealNode>> TryParse(ImmutableList<IToken> tokens) =>
      tokens.HeadOrNone().ToEither<ParseException>(() => new EmptySequenceException())
        .Match<Either<ParseException, ParseResult<RealNode>>>(
          Left: e => e,
          Right: t =>
          {
            if (t is RealKeywordToken)
              return new ParseResult<RealNode>(new RealNode(), tokens.Skip(1).ToImmutableList());
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