namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class GtOperatorToken : OperatorToken
  {
    public const string LexemeValue = ">";

    public override string Lexeme => LexemeValue;

    public GtOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
