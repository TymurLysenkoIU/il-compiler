namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration
{
  public class ArrayKeywordToken : KeywordToken
  {
    public override string Lexeme => "array";

    public ArrayKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
