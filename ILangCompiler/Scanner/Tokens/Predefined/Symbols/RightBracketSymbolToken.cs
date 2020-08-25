namespace ILangCompiler.Scanner.Tokens.Predefined.Symbols
{
  public class RightBracketSymbolToken : SymbolToken
  {
    public override string Lexeme => "]";

    public RightBracketSymbolToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
