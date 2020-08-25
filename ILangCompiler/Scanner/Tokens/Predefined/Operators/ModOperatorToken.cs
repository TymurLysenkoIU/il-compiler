namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class ModOperatorToken : OperatorToken
  {
    public const string LexemeValue = "%";

    public override string Lexeme => LexemeValue;

    public ModOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
