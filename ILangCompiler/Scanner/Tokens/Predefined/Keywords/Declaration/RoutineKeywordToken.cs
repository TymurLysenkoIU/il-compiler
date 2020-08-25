namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration
{
  public class RoutineKeywordToken : KeywordToken
  {
    public const string LexemeValue = "routine";

    public override string Lexeme => LexemeValue;

    public RoutineKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}