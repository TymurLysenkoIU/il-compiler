
namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.PrimitiveTypes
{
  public class BooleanKeywordToken : KeywordToken
  {
    public const string LexemeValue = "boolean";

    public override string Lexeme => LexemeValue;

    public BooleanKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
