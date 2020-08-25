namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Constructions
{
  public class WhileKeywordToken : KeywordToken
  {
    public const string LexemeValue = "while";

    public override string Lexeme => LexemeValue;

    public WhileKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
