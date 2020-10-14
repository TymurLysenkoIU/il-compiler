using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.Parser.AST.TypeTable.TypeRepresentation;

namespace ILangCompiler.Parser.AST.TypeTable
{
    public abstract class ScalarType : IEntityType
    {
        public ITypeRepresentation EntityType;

        protected ScalarType(ITypeRepresentation entityType)
        {
            EntityType = entityType;
        }
    }
}