namespace ILangCompiler.Scanner.Tokens.Predefined.Symbols
{
  public class RangeSymbolToken : SymbolToken
  {
    public override string Lexeme => "..";

    public RangeSymbolToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
