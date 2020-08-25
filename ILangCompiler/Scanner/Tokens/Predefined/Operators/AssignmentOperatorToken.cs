namespace ILangCompiler.Scanner.Tokens.Predefined.Operators
{
  public class AssignmentOperatorToken : OperatorToken
  {
    public override string Lexeme => ":=";

    public AssignmentOperatorToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
