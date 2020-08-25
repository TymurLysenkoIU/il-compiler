namespace ILangCompiler.Scanner.Tokens.Predefined.Symbols
{
  public class ColonSymbolToken : SymbolToken
  {
    public override string Lexeme => ":";

    public ColonSymbolToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
