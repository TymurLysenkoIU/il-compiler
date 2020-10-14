namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords.Constructions
{
    public class ReturnKeywordToken : KeywordToken
    {
        public const string LexemeValue = "return";

        public override string Lexeme => LexemeValue;

        public ReturnKeywordToken(
            uint absolutePosition,
            uint lineNumber,
            uint positionInLine
        ) : base(absolutePosition, lineNumber, positionInLine)
        {

        }
    }
}