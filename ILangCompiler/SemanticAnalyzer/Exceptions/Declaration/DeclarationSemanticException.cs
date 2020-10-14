using ILangCompiler.Parser.AST;

namespace ILangCompiler.SemanticAnalyzer.Exceptions.Declaration
{
    public class DeclarationSemanticException : SemanticException
    {
        public DeclarationSemanticException(IAstNode node, string message) : base(node, message)
        {
        }
    }
}