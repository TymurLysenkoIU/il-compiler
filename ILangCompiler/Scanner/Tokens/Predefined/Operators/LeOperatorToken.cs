namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class LeOperatorToken : OperatorToken
  {
    public override string Lexeme => "<=";

    public LeOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
