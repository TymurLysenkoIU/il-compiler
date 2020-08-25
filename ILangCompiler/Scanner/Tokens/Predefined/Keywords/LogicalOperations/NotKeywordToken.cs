namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.LogicalOperations
{
  public class NotKeywordToken : KeywordToken
  {
    public const string LexemeValue = "not";

    public override string Lexeme => LexemeValue;

    public NotKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}