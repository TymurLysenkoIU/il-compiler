using ILangCompiler.Parser.Combinators;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords;
using LanguageExt;

namespace ILangCompiler.Parser.Keywords
{
  public class EndKeywordParser : ConstantParser<Unit, EndKeywordToken>
  {
  }
}