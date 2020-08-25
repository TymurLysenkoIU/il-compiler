namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class MinusOperatorToken : OperatorToken
  {
    public override string Lexeme => "-";

    public MinusOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
