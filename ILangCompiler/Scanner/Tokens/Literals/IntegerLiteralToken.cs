namespace ILangCompiler.Scanner.Tokens.Literals
{
  public class IntegerLiteralToken : LiteralToken
  {
    private IntegerLiteralToken(
      string lexeme,
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ): base(lexeme, absolutePosition, lineNumber, positionInLine)
    {
      // TODO: validation
    }
  }
}