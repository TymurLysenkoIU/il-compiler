namespace ILangCompiler.Scanner.Tokens
{
  public class CharToken : IToken
  {
    public char C { get; }

    public CharToken(char c)
    {
      C = c;
    }

    public string Lexeme => C.ToString();
  }
}