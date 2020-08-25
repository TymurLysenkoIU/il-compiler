namespace ILangCompiler.Scanner.Tokens.Predefined.Symbols
{
  public class ComaSymbolToken : SymbolToken
  {
    public const string LexemeValue = ",";

    public override string Lexeme => LexemeValue;

    public ComaSymbolToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
