namespace ILangCompiler.Parser.Exceptions
{
  public class EitherParseException : ParseException
  {
    public EitherParseException(string description = "No parser matched the sequence") : base(description)
    {
    }
  }
}