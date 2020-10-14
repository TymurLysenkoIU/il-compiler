namespace ILangCompiler.Parser.AST.TypeTable.TypeRepresentation
{
    public class TypeAliasRepresentation : ITypeRepresentation
    {
        public string Alias { get; }

        public TypeAliasRepresentation(string @alias)
        {
            Alias = alias;
        }
    }
}