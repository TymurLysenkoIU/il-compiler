namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords
{
  public class FalseKeywordToken : KeywordToken
  {
    public const string LexemeValue = "false";

    public override string Lexeme => LexemeValue;

    public FalseKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}