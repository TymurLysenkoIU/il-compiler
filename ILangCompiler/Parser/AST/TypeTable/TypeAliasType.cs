using ILangCompiler.Parser.AST.TypeTable.TypeRepresentation;

namespace ILangCompiler.Parser.AST.TypeTable
{
    public class TypeAliasType : ScalarType
    {
        public TypeAliasType(ITypeRepresentation entityType) : base(entityType)
        {
        }
    }
}