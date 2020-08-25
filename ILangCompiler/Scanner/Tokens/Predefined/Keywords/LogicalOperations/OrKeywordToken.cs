
namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.LogicalOperations
{
  public class OrKeywordToken : KeywordToken
  {
    public override string Lexeme => "or";

    public OrKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
