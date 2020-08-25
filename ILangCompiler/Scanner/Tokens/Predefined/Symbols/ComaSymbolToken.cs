namespace ILangCompiler.Scanner.Tokens.Predefined.Symbols
{
  public class ComaSymbolToken : SymbolToken
  {
    public override string Lexeme => ",";

    public ComaSymbolToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
