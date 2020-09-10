using System.Collections.Immutable;
using ILangCompiler.AST.Types.PrimitiveTypes;
using ILangCompiler.Parser.Abstractions;
using ILangCompiler.Parser.Combinators;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.PrimitiveTypes;
using LanguageExt;

namespace ILangCompiler.Parser.Types.PrimitiveTypes
{
  public class BooleanParser :
    ConstantParser<BooleanNode, BooleanKeywordToken>,
    IParser<ParseException, IPrimitiveTypeNode>
  {
    Either<ParseException, ParseResult<IPrimitiveTypeNode>> IParser<ParseException, IPrimitiveTypeNode>.TryParse(
      ImmutableList<IToken> tokens
    ) =>
      TryParse(tokens).Map(pr => new ParseResult<IPrimitiveTypeNode>(
        pr.Result,
        pr.RestTokens
      ));
  }
}