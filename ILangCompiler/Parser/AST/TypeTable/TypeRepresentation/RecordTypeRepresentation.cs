using System.Collections.Generic;
using System.Collections.Immutable;

namespace ILangCompiler.Parser.AST.TypeTable.TypeRepresentation
{
    public class RecordTypeRepresentation : ITypeRepresentation
    {
        public ImmutableDictionary<string, ITypeRepresentation> FieldsWithTypes { get; }

        public RecordTypeRepresentation(IDictionary<string, ITypeRepresentation> fieldsWithTypes)
        {
            FieldsWithTypes = fieldsWithTypes.ToImmutableDictionary();
        }
    }
}