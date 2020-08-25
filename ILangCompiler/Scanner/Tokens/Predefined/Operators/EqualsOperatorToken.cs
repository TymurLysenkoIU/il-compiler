namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class EqualsOperatorToken : OperatorToken
  {
    public const string LexemeValue = "=";

    public override string Lexeme => LexemeValue;

    public EqualsOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
