namespace ILangCompiler.Scanner.Tokens.Literals
{
  public class RealLiteralToken : LiteralToken
  {
    private RealLiteralToken(
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