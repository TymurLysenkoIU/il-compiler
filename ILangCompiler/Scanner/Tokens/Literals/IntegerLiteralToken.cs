using System.Text.RegularExpressions;
using LanguageExt;

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
    }

    public static readonly Regex IntegerPatten =
      new Regex(@"^[0-9]+$", RegexOptions.Compiled);

    public static Option<IToken> FromString(
      string s,
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) =>
      IntegerPatten.IsMatch(s)
        ? new IntegerLiteralToken(
          s,
          absolutePosition,
          lineNumber,
          positionInLine
        )
        : Option<IToken>.None;
  }
}