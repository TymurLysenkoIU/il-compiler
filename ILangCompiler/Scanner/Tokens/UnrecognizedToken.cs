using LanguageExt;

namespace ILangCompiler.Scanner.Tokens
{
  public class UnrecognizedToken : IToken
  {
    public string Lexeme { get; }
    public uint AbsolutePosition { get; }
    public uint LineNumber { get; }
    public uint PositionInLine { get; }

    public UnrecognizedToken(string lexeme, uint absolutePosition, uint lineNumber, uint positionInLine)
    {
      Lexeme = lexeme;
      AbsolutePosition = absolutePosition;
      LineNumber = lineNumber;
      PositionInLine = positionInLine;
    }

    public static Option<IToken> FromString(
      string s,
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) =>
      string.IsNullOrEmpty(s)
        ? Option<IToken>.None
        : new UnrecognizedToken(
            s,
            absolutePosition,
            lineNumber,
            positionInLine
          );
  }
}