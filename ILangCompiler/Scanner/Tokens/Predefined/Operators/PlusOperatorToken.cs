namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class PlusOperatorToken : OperatorToken
  {
    public override string Lexeme => "+";

    public PlusOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
