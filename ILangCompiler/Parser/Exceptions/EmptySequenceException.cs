namespace ILangCompiler.Parser.Exceptions
{
    public class EmptySequenceException : ParseException
    {
        public EmptySequenceException(
            string description = "Input has came to its end"
        ) : base(description)
        {

        }
    }
}