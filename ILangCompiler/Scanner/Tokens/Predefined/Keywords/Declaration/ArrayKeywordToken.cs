namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration
{
  public class ArrayKeywordToken : KeywordToken
  {
    public const string LexemeValue = "array";

    public override string Lexeme => LexemeValue;

    public ArrayKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
