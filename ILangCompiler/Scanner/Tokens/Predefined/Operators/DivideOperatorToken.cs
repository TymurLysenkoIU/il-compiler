namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class DivideOperatorToken : OperatorToken
  {
    public override string Lexeme => "/";

    public DivideOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
