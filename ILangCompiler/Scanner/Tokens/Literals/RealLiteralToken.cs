using System.Text.RegularExpressions;
using LanguageExt;

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
    }

    public static readonly Regex RealPatten =
      new Regex(@"[0-9]+\.[0-9]+$", RegexOptions.Compiled);

    public static Option<IToken> FromString(
      string s,
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) =>
      RealPatten.IsMatch(s)
        ? new RealLiteralToken(
          s,
          absolutePosition,
          lineNumber,
          positionInLine
        )
        : Option<IToken>.None;
  }
}