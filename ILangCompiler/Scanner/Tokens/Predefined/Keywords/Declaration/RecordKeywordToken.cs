namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration
{
  public class RecordKeywordToken : KeywordToken
  {
    public const string LexemeValue = "record";

    public override string Lexeme => LexemeValue;

    public RecordKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
