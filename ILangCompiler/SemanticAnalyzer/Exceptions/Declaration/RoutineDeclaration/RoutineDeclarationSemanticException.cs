using ILangCompiler.Parser.AST;

namespace ILangCompiler.SemanticAnalyzer.Exceptions.Declaration.RoutineDeclaration
{
    public class RoutineDeclarationSemanticException : DeclarationSemanticException
    {
        public RoutineDeclarationSemanticException(IAstNode node, string message) : base(node, message)
        {
        }
    }
}