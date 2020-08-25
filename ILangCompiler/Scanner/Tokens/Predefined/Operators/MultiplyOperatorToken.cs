namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class MultiplyOperatorToken : OperatorToken
  {
    public const string LexemeValue = "*";

    public override string Lexeme => LexemeValue;

    public MultiplyOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
