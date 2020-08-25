namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class GeOperatorToken : OperatorToken
  {
    public override string Lexeme => ">=";

    public GeOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
