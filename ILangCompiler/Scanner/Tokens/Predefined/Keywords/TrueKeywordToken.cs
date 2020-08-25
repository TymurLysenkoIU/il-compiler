namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords
{
  public class TrueKeywordToken : KeywordToken
  {
    public override string Lexeme => "true";

    public TrueKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}