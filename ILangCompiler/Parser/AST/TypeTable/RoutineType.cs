using System.Collections.Immutable;
using ILangCompiler.Parser.AST.Declarations;
using ILangCompiler.Parser.AST.TypeTable.TypeRepresentation;
using LanguageExt;
using static LanguageExt.Prelude;


namespace ILangCompiler.Parser.AST.TypeTable
{
    public class RoutineType : IEntityType
    {
        public Option<ITypeRepresentation> ReturnType = None;
        public ImmutableList<ITypeRepresentation> ParametersTypes = ImmutableList<ITypeRepresentation>.Empty;

        public RoutineType()
        {
        }

        public RoutineType(ITypeRepresentation returnType)
        {
            ReturnType = Some(returnType);
        }

        public RoutineType(ITypeRepresentation returnType, ImmutableList<ITypeRepresentation> parametersTypes)
        {
            ReturnType = Some(returnType);
            ParametersTypes = parametersTypes;
        }

        public RoutineType(ImmutableList<ITypeRepresentation> parametersTypes)
        {
            ParametersTypes = parametersTypes;
        }

        public RoutineType(Option<ITypeRepresentation> returnType, ImmutableList<ITypeRepresentation> parametersTypes)
        {
            ReturnType = returnType;
            ParametersTypes = parametersTypes;
        }
    }
}