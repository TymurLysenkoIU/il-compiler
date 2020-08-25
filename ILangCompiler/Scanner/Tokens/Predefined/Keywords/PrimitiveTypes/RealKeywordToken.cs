
namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.PrimitiveTypes
{
  public class RealKeywordToken : KeywordToken
  {
    public const string LexemeValue = "real";

    public override string Lexeme => LexemeValue;

    public RealKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
