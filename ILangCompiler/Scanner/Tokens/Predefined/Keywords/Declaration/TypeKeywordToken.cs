namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration
{
  public class TypeKeywordToken : KeywordToken
  {
    public override string Lexeme => "type";

    public TypeKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}