using ILangCompiler.Parser.AST;

namespace ILangCompiler.SemanticAnalyzer.Exceptions.Declaration.SimpleDeclaration.TypeDeclaration
{
    public class TypeDeclarationSemanticException : SimpleDeclarationSemanticException
    {
        public TypeDeclarationSemanticException(IAstNode node, string message) : base(node, message)
        {
        }
    }
}