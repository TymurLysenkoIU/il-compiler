namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords
{
  public class TrueKeywordToken : KeywordToken
  {
    public const string LexemeValue = "true";

    public override string Lexeme => LexemeValue;

    public TrueKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}