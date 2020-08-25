namespace ILangCompiler.Scanner.Tokens.Predefined.Symbols
{
  public class LeftBracketSymbolToken : SymbolToken
  {
    public override string Lexeme => "[";

    public LeftBracketSymbolToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
