namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public abstract class OperatorToken : PredefinedToken
  {
    public OperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}