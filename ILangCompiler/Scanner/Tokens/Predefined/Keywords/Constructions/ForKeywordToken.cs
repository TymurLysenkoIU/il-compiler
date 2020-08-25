
namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Constructions
{
  public class ForKeywordToken : KeywordToken
  {
    public override string Lexeme => "for";

    public ForKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
