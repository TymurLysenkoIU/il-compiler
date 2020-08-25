
namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Constructions
{
  public class WhileKeywordToken : KeywordToken
  {
    public override string Lexeme => "while";

    public WhileKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
