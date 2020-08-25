namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class GeOperatorToken : OperatorToken
  {
    public const string LexemeValue = ">=";

    public override string Lexeme => LexemeValue;

    public GeOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
