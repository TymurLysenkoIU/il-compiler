
namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.PrimitiveTypes
{
  public class BooleanKeywordToken : KeywordToken
  {
    public override string Lexeme => "boolean";

    public BooleanKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
