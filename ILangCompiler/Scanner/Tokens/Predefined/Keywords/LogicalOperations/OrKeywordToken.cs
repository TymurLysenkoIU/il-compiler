
namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.LogicalOperations
{
  public class OrKeywordToken : KeywordToken
  {
    public const string LexemeValue = "or";

    public override string Lexeme => LexemeValue;

    public OrKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
