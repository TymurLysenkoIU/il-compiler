namespace ILangCompiler.Scanner.Tokens.Predefined.Symbols
{
  public class LeftBracketSymbolToken : SymbolToken
  {
    public const string LexemeValue = "[";

    public override string Lexeme => LexemeValue;

    public LeftBracketSymbolToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
