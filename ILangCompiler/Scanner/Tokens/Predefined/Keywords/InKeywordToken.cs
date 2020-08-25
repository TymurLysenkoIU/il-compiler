
namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords
{
  public class InKeywordToken : KeywordToken
  {
    public const string LexemeValue = "in";

    public override string Lexeme => LexemeValue;

    public InKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
