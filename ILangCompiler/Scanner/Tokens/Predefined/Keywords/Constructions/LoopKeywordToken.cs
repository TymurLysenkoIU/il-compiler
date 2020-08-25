namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Constructions
{
  public class LoopKeywordToken : KeywordToken
  {
    public const string LexemeValue = "loop";

    public override string Lexeme => LexemeValue;

    public LoopKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
