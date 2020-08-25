
namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Constructions
{
  public class LoopKeywordToken : KeywordToken
  {
    public override string Lexeme => "loop";

    public LoopKeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
