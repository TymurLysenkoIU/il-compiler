namespace ILangCompiler.Scanner.Tokens.Predefined.Symbols
{
  public class NewLineSymbolToken : SymbolToken
  {
    public const string LexemeValue = "\n";

    public override string Lexeme => LexemeValue;

    public NewLineSymbolToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}