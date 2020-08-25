namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class DivideOperatorToken : OperatorToken
  {
    public const string LexemeValue = "/";

    public override string Lexeme => LexemeValue;

    public DivideOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
