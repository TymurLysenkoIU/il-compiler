namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords
{
  public class ReverseKeywordToken : KeywordToken
  {
    public const string LexemeValue = "reverse";

    public override string Lexeme => LexemeValue;

    public ReverseKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
