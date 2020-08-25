
namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Constructions
{
  public class IfKeywordToken : KeywordToken
  {
    public override string Lexeme => "if";

    public IfKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
