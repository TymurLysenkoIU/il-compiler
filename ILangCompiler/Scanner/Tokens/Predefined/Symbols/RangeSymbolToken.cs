namespace ILangCompiler.Scanner.Tokens.Predefined.Symbols
{
  public class RangeSymbolToken : SymbolToken
  {
    public const string LexemeValue = "..";

    public override string Lexeme => LexemeValue;

    public RangeSymbolToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
