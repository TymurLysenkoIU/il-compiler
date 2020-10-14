using ILangCompiler.Parser.AST.TypeTable.TypeRepresentation;

namespace ILangCompiler.Parser.AST.TypeTable
{
    public class VariableType : ScalarType
    {
        public VariableType(ITypeRepresentation entityType) : base(entityType)
        {
        }
    }
}