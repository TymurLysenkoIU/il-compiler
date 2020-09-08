using System.Collections.Immutable;
using ILangCompiler.AST;
using ILangCompiler.AST.Types.PrimitiveTypes;
using ILangCompiler.Parser.Abstractions;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using LanguageExt;

namespace ILangCompiler.Parser.Types.PrimitiveTypes
{
  // public abstract class CastingPrimitiveTypeParser<TE, TR> : IParser<TE, IPrimitiveTypeNode>, IParser<TE, TR>
  //   where TE : ParseException
  //   where TR : IPrimitiveTypeNode
  // {
  //   public abstract Either<TE, ParseResult<TR>> TryParse(ImmutableList<IToken> tokens);
  //
  //   Either<TE, ParseResult<IPrimitiveTypeNode>> IParser<TE, IPrimitiveTypeNode>.TryParse(
  //     ImmutableList<IToken> tokens
  //   ) =>
  //     TryParse(tokens).Map(pr => new ParseResult<IPrimitiveTypeNode>(
  //       pr.Result,
  //       pr.RestTokens
  //     ));
  // }
}