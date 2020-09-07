using System;

namespace ILangCompiler.Parser.Exceptions
{
    public class ParseException : Exception
    {
        public ParseException(string message) : base(message)
        {
        }
    }
}