using LanguageExt;

namespace ILangCompiler.Parser.AST.TypeTable
{
    public interface ITypeTable<TET> : IScopedTable<TET, string>
        where TET : IEntityType
    {
    }
}