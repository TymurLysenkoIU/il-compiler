namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class PlusOperatorToken : OperatorToken
  {
    public const string LexemeValue = "+";

    public override string Lexeme => LexemeValue;

    public PlusOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
