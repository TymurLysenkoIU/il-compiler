
namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.LogicalOperations
{
  public class XorKeywordToken : KeywordToken
  {
    public const string LexemeValue = "xor";

    public override string Lexeme => LexemeValue;

    public XorKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
