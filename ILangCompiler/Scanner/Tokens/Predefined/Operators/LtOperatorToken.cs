namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class LtOperatorToken : OperatorToken
  {
    public const string LexemeValue = "<";

    public override string Lexeme => LexemeValue;

    public LtOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
