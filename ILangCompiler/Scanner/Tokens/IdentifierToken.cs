using System.Text.RegularExpressions;
using LanguageExt;

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
      Lexeme = lexeme;
      AbsolutePosition = absolutePosition;
      LineNumber = lineNumber;
      PositionInLine = positionInLine;
    }

    public static readonly Regex IdentifierPatten =
      new Regex(@"^[a-zA-Z][a-zA-Z0-9]*$", RegexOptions.Compiled);

    public static Option<IToken> FromString(
      string s,
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) =>
      IdentifierPatten.IsMatch(s)
        ? new IdentifierToken(
          s,
          absolutePosition,
          lineNumber,
          positionInLine
        )
        : Option<IToken>.None;
  }
}