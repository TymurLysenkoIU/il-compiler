
namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords
{
  public class InKeywordToken : KeywordToken
  {
    public override string Lexeme => "in";

    public InKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
