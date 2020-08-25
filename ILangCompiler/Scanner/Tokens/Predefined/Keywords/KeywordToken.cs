namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords
{
  public abstract class KeywordToken : PredefinedToken
  {
    public KeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}