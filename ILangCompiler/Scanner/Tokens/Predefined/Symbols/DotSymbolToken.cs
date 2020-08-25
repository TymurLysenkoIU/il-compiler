namespace ILangCompiler.Scanner.Tokens.Predefined.Symbols
{
  public class DotSymbolToken : SymbolToken
  {
    public const string LexemeValue = ".";

    public override string Lexeme => LexemeValue;

    public DotSymbolToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
