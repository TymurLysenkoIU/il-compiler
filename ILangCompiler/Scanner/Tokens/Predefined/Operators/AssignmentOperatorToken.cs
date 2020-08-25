namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class AssignmentOperatorToken : OperatorToken
  {
    public const string LexemeValue = ":=";

    public override string Lexeme => LexemeValue;

    public AssignmentOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
