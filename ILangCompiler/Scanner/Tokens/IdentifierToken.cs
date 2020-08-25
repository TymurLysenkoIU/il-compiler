namespace ILangCompiler.Scanner.Tokens
{
  public class IdentifierToken : IToken
  {
    public string Lexeme { get; }
    public uint AbsolutePosition { get; }
    public uint LineNumber { get; }
    public uint PositionInLine { get; }

    private IdentifierToken(string lexeme, uint absolutePosition, uint lineNumber, uint positionInLine)
    {
      // TODO: validation
      Lexeme = lexeme;
      AbsolutePosition = absolutePosition;
      LineNumber = lineNumber;
      PositionInLine = positionInLine;
    }
  }
}