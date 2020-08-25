
namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.PrimitiveTypes
{
  public class IntegerKeywordToken : KeywordToken
  {
    public override string Lexeme => "integer";

    public IntegerKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
