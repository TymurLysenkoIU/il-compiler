using System;

namespace ILangCompiler.Parser.Exceptions
{
  public class ParseException : Exception
  {
    public ParseException(string description) : base(description)
    {
    }

    public ParseException() : base("No parser matched the sequence")
    {
    }
  }
}