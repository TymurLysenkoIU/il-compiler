
namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Constructions
{
  public class ThenKeywordToken : KeywordToken
  {
    public override string Lexeme => "then";

    public ThenKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
