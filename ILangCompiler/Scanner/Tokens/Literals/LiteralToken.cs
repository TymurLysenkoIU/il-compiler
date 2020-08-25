namespace ILangCompiler.Scanner.Tokens.Literals
{
  public abstract class LiteralToken : IToken
  {
    public string Lexeme { get; }
    public uint AbsolutePosition { get; }
    public uint LineNumber { get; }
    public uint PositionInLine { get; }

    public LiteralToken(string lexeme, uint absolutePosition, uint lineNumber, uint positionInLine)
    {
      Lexeme = lexeme;
      AbsolutePosition = absolutePosition;
      LineNumber = lineNumber;
      PositionInLine = positionInLine;
    }
  }
}