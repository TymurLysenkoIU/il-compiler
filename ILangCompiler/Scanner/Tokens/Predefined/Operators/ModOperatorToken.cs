namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class ModOperatorToken : OperatorToken
  {
    public override string Lexeme => "%";

    public ModOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
