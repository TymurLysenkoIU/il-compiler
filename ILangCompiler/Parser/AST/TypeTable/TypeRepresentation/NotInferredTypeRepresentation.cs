using ILangCompiler.Parser.AST.Expressions;

namespace ILangCompiler.Parser.AST.TypeTable.TypeRepresentation
{
    public class NotInferredTypeRepresentation : ITypeRepresentation
    {
        public ExpressionNode Expression { get; }

        public NotInferredTypeRepresentation(ExpressionNode expression)
        {
            Expression = expression;
        }
    }
}