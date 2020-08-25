namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Constructions
{
  public class ElseKeywordToken : KeywordToken
  {
    public const string LexemeValue = "else";

    public override string Lexeme => LexemeValue;

    public ElseKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
