namespace ILangCompiler.Scanner.Tokens.Predefined.Symbols
{
  public class LeftParenthSymbolToken : SymbolToken
  {
    public const string LexemeValue = "(";

    public override string Lexeme => LexemeValue;

    public LeftParenthSymbolToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
