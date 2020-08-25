namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration
{
  public class RoutineKeywordToken : KeywordToken
  {
    public override string Lexeme => "routine";

    public RoutineKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}