namespace ILangCompiler.Scanner.Tokens.Predefined.Symbols
{
  public class RightParenthSymbolToken : SymbolToken
  {
    public override string Lexeme => ")";

    public RightParenthSymbolToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
