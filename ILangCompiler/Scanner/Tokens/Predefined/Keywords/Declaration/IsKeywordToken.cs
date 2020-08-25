namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration
{
  public class IsKeywordToken : KeywordToken
  {
    public override string Lexeme => "is";

    public IsKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}