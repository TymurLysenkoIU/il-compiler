
namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.PrimitiveTypes
{
  public class IntegerKeywordToken : KeywordToken
  {
    public const string LexemeValue = "integer";

    public override string Lexeme => LexemeValue;

    public IntegerKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
