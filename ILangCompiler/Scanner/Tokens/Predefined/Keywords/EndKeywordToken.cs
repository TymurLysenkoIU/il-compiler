namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords
{
  public class EndKeywordToken : KeywordToken
  {
    public override string Lexeme => "end";

    public EndKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}