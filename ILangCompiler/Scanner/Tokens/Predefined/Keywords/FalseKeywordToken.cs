namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords
{
  public class FalseKeywordToken : KeywordToken
  {
    public override string Lexeme => "false";

    public FalseKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}