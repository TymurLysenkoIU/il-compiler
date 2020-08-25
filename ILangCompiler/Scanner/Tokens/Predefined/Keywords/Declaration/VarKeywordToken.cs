namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration
{
  public class VarKeywordToken : KeywordToken
  {
    public const string LexemeValue = "var";

    public override string Lexeme => LexemeValue;

    public VarKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}