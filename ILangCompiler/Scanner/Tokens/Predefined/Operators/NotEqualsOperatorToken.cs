namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class NotEqualsOperatorToken : OperatorToken
  {
    public override string Lexeme => "/=";

    public NotEqualsOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
