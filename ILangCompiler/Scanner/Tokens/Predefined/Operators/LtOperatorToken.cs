namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class LtOperatorToken : OperatorToken
  {
    public override string Lexeme => "<";

    public LtOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
