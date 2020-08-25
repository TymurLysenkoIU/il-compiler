namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class MinusOperatorToken : OperatorToken
  {
    public const string LexemeValue = "-";

    public override string Lexeme => LexemeValue;

    public MinusOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
