namespace ILangCompiler.Scanner.Tokens.Predefined.Symbols
{
  public class RightBracketSymbolToken : SymbolToken
  {
    public const string LexemeValue = "]";

    public override string Lexeme => LexemeValue;

    public RightBracketSymbolToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
