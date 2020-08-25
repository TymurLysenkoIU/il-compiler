namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration
{
  public class RecordKeywordToken : KeywordToken
  {
    public override string Lexeme => "record";

    public RecordKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
