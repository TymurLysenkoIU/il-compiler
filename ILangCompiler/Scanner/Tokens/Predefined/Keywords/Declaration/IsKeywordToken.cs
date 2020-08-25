namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration
{
  public class IsKeywordToken : KeywordToken
  {
    public const string LexemeValue = "is";

    public override string Lexeme => LexemeValue;

    public IsKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}