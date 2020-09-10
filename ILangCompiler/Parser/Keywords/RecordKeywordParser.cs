using ILangCompiler.Parser.Combinators;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration;
using LanguageExt;

namespace ILangCompiler.Parser.Keywords
{
  public class RecordKeywordParser : ConstantParser<Unit, RecordKeywordToken>
  {
  }
}