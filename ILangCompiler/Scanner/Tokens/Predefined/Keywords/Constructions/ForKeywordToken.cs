namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Constructions
{
  public class ForKeywordToken : KeywordToken
  {
    public const string LexemeValue = "for";

    public override string Lexeme => LexemeValue;

    public ForKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
