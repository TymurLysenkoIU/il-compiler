namespace ILangCompiler.Scanner.Tokens.Predefined.Symbols
{
  public abstract class SymbolToken : PredefinedToken
  {
    public SymbolToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}