namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class EqualsOperatorToken : OperatorToken
  {
    public override string Lexeme => "=";

    public EqualsOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
