namespace ILangCompiler.Scanner.Tokens
{
  public class CommentToken : IToken
  {
    public const string CommentStartLexeme = "//";

    public string Lexeme { get; }
    public uint AbsolutePosition { get; }
    public uint LineNumber { get; }
    public uint PositionInLine { get; }

    public CommentToken(string lexeme, uint absolutePosition, uint lineNumber, uint positionInLine)
    {
      Lexeme = lexeme;
      AbsolutePosition = absolutePosition;
      LineNumber = lineNumber;
      PositionInLine = positionInLine;
    }
  }
}