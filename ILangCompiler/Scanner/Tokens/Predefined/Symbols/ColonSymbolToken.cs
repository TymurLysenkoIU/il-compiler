namespace ILangCompiler.Scanner.Tokens.Predefined.Symbols
{
  public class ColonSymbolToken : SymbolToken
  {
    public const string LexemeValue = ":";

    public override string Lexeme => LexemeValue;

    public ColonSymbolToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
