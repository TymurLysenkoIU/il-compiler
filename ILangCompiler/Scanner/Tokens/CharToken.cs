namespace ILangCompiler.Scanner.Tokens
{
  public class CharToken : IToken
  {
    public char C { get; }

    public string Lexeme => C.ToString();
    public uint AbsolutePosition { get; }
    public uint LineNumber { get; }
    public uint PositionInLine { get; }

    public CharToken(
      char c,
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    )
    {
      C = c;
      AbsolutePosition = absolutePosition;
      LineNumber = lineNumber;
      PositionInLine = positionInLine;
    }
  }
}