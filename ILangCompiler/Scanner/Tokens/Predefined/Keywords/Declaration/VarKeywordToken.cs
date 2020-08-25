namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration
{
  public class VarKeywordToken : KeywordToken
  {
    public override string Lexeme => "var";

    public VarKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}