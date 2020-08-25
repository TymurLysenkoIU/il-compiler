namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Constructions
{
  public class ThenKeywordToken : KeywordToken
  {
    public const string LexemeValue = "then";

    public override string Lexeme => LexemeValue;

    public ThenKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
