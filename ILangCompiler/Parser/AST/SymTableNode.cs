using ILangCompiler.Scanner.Tokens;

namespace ILangCompiler.Parser.AST
{
    public class SymTableNode
    {
        public static string generator = "ID_";
        public static int generator_int = 0;
        
        public IdentifierToken Identifier;
        public string UniqueName;

        public SymTableNode(IdentifierToken identifier)
        {
            Identifier = identifier;
            
            UniqueName = generator.Clone().ToString() + generator_int.ToString();
            generator_int += 1;
        }

        public bool TheSame(IdentifierToken identifier)
        {
            return identifier.Lexeme == Identifier.Lexeme;
        }
        
    }
    
    
    
    
}