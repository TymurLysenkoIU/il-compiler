using ILangCompiler.Parser.AST.Declarations.Types.PrimitiveTypes;

namespace ILangCompiler.Parser.AST.TypeTable.TypeRepresentation
{
    public class PrimitiveTypeRepresentation : ITypeRepresentation
    {
        public PrimitiveTypeNode TypeNode;

        public PrimitiveTypeRepresentation(PrimitiveTypeNode typeNode)
        {
            TypeNode = typeNode;
        }
    }
}