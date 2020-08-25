namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class GtOperatorToken : OperatorToken
  {
    public override string Lexeme => ">";

    public GtOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
