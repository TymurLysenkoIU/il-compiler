using ILangCompiler.Parser.AST;

namespace ILangCompiler.SemanticAnalyzer.Exceptions.Declaration.SimpleDeclaration
{
    public class SimpleDeclarationSemanticException : DeclarationSemanticException
    {
        public SimpleDeclarationSemanticException(IAstNode node, string message) : base(node, message)
        {
        }
    }
}