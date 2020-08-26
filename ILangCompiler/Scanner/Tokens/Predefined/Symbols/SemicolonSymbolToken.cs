namespace ILangCompiler.Scanner.Tokens.Predefined.Symbols
{
  public class SemicolonSymbolToken : SymbolToken
  {
    public const string LexemeValue = ";";

    public override string Lexeme => LexemeValue;

    public SemicolonSymbolToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}