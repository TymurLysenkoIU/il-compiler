namespace ILangCompiler.Scanner.Tokens.Predefined.Symbols
{
  public class RightParenthSymbolToken : SymbolToken
  {
    public const string LexemeValue = ")";

    public override string Lexeme => LexemeValue;

    public RightParenthSymbolToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
