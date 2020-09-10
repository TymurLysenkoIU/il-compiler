using System.Collections.Immutable;
using ILangCompiler.AST;
using ILangCompiler.Parser.Abstractions;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using LanguageExt;

namespace ILangCompiler.Parser.Combinators
{
  public abstract class PipelineParser<TPE, TPR, TE, TR> : IParser<TE, TR>
    where TPE : TE
    // where TPR : TR

    where TE : ParseException, new()
    // where TR : IAstNode
  {
    protected ImmutableArray<IParser<TPE, TPR>> Parsers;

    public PipelineParser(ImmutableArray<IParser<TPE, TPR>> parsers)
    {
      Parsers = parsers;
    }

    // protected abstract TR Flatten(); // TODO: decide the signature

    public Either<TE, ParseResult<TR>> TryParse(ImmutableList<IToken> tokens)
    {

    }
  }
}