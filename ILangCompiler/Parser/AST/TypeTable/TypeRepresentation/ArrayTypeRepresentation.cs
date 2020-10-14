namespace ILangCompiler.Parser.AST.TypeTable.TypeRepresentation
{
    public class ArrayTypeRepresentation : ITypeRepresentation
    {
        public ITypeRepresentation UnderlyingType { get; }

        public ArrayTypeRepresentation(ITypeRepresentation underlyingType)
        {
            UnderlyingType = underlyingType;
        }
    }
}