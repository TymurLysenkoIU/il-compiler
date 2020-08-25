namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords
{
  public class EndKeywordToken : KeywordToken
  {
    public const string LexemeValue = "end";

    public override string Lexeme => LexemeValue;

    public EndKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}