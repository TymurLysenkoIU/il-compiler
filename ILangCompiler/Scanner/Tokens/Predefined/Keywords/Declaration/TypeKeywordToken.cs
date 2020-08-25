namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration
{
  public class TypeKeywordToken : KeywordToken
  {
    public const string LexemeValue = "type";

    public override string Lexeme => LexemeValue;

    public TypeKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}