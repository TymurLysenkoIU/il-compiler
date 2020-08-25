
namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.LogicalOperations
{
  public class AndKeywordToken : KeywordToken
  {
    public const string LexemeValue = "and";

    public override string Lexeme => LexemeValue;

    public AndKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
