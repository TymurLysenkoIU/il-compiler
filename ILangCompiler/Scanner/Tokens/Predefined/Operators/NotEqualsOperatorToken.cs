namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class NotEqualsOperatorToken : OperatorToken
  {
    public const string LexemeValue = "/=";

    public override string Lexeme => LexemeValue;

    public NotEqualsOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
