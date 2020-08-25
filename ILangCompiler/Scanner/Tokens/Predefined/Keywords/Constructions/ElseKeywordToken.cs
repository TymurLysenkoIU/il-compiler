
namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Constructions
{
  public class ElseKeywordToken : KeywordToken
  {
    public override string Lexeme => "else";

    public ElseKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
