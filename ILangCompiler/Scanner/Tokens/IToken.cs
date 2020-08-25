namespace ILangCompiler.Scanner.Tokens
{
  public interface IToken
  {
    public string Lexeme { get; }
    public uint AbsolutePosition { get; }
    public uint LineNumber { get; }
    public uint PositionInLine { get; }
  }
}