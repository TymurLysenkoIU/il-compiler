using System.Collections.Immutable;
using System.Linq;
using ILangCompiler.Parser.AST.Declarations;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.Parser.AST.TypeTable.TypeRepresentation;

namespace ILangCompiler.Parser.AST.TypeTable
{
    public static class TypeConstructorExtensions
    {
        public static RoutineType ToRoutineType(this RoutineDeclarationNode node) =>
            new RoutineType(
                node.ReturnType
                    .Map(t => t.ToTypeRepresentation()),
                node.Parameters
                    .Select(pn => pn.Type.ToTypeRepresentation())
                    .ToImmutableList()
            );

        public static VariableType ToVariableType(this VariableDeclarationNode node) =>
            new VariableType(node.ToTypeRepresentation());

        public static TypeAliasType ToTypeAliasType(this TypeDeclarationNode node) =>
            new TypeAliasType(node.Type.ToTypeRepresentation());
    }
}