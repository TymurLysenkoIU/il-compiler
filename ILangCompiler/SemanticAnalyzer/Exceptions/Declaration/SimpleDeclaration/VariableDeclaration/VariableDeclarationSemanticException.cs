using ILangCompiler.Parser.AST;

namespace ILangCompiler.SemanticAnalyzer.Exceptions.Declaration.SimpleDeclaration.VariableDeclaration
{
    public class VariableDeclarationSemanticException : SimpleDeclarationSemanticException
    {
        public VariableDeclarationSemanticException(IAstNode node, string message) : base(node, message)
        {
        }
    }
}