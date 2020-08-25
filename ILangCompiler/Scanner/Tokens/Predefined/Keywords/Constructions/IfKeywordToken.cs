namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Constructions
{
  public class IfKeywordToken : KeywordToken
  {
    public const string LexemeValue = "if";

    public override string Lexeme => LexemeValue;

    public IfKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
