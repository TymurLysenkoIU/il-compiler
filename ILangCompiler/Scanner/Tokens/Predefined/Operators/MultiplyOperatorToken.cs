namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class MultiplyOperatorToken : OperatorToken
  {
    public override string Lexeme => "*";

    public MultiplyOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
