namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class LeOperatorToken : OperatorToken
  {
    public const string LexemeValue = "<=";

    public override string Lexeme => LexemeValue;

    public LeOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
