namespace ILangCompiler.Scanner.Tokens.Predefined
{
  public abstract class PredefinedToken : IToken
  {
    public abstract string Lexeme { get; }
    public uint AbsolutePosition { get; }
    public uint LineNumber { get; }
    public uint PositionInLine { get; }

    public PredefinedToken(uint absolutePosition, uint lineNumber, uint positionInLine)
    {
      AbsolutePosition = absolutePosition;
      LineNumber = lineNumber;
      PositionInLine = positionInLine;
    }
  }
}