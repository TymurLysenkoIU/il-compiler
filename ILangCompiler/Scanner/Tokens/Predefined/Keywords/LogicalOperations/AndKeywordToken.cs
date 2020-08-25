
namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.LogicalOperations
{
  public class AndKeywordToken : KeywordToken
  {
    public override string Lexeme => "and";

    public AndKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
