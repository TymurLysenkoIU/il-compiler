namespace ILangCompiler.Scanner.Tokens.Predefined.Symbols
{
  public class LeftParenthSymbolToken : SymbolToken
  {
    public override string Lexeme => "(";

    public LeftParenthSymbolToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
