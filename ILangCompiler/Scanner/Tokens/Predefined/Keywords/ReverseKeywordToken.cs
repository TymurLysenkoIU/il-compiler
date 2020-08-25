namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords
{
  public class ReverseKeywordToken : KeywordToken
  {
    public override string Lexeme => "reverse";

    public ReverseKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
