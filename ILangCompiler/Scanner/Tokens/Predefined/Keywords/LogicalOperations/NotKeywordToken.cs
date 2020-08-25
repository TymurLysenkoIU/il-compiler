namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.LogicalOperations
{
  public class NotKeywordToken : KeywordToken
  {
    public override string Lexeme => "not";

    public NotKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}