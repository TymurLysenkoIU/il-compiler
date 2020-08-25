namespace ILangCompiler.Scanner.Tokens.Predefined.Symbols
{
  public class DotSymbolToken : SymbolToken
  {
    public override string Lexeme => ".";

    public DotSymbolToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
