using System.Linq;
using System.Text;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.Parser.AST.Declarations.Types.PrimitiveTypes;
using ILangCompiler.Parser.AST.Expressions;

namespace ILangCompiler.Parser.AST.TypeTable.TypeRepresentation
{
    public static class TypeRepresentationExtensions
    {
        public static ITypeRepresentation ToTypeRepresentation(this VariableDeclarationNode variableDeclarationNode) =>
            variableDeclarationNode.Type
                .Map(t => t.ToTypeRepresentation())
                .IfNone(() => variableDeclarationNode.Expression
                    .Map<ITypeRepresentation>(e =>
                        new NotInferredTypeRepresentation(e)
                    )
                    .IfNone(new NoTypeRepresentation())
                );

        public static ITypeRepresentation ToTypeRepresentation(this TypeNode typeNode)
        {
            if (typeNode.PrimitiveType != null)
            {
                return new PrimitiveTypeRepresentation(typeNode.PrimitiveType);
            }

            if (typeNode.ArrayType != null)
            {
                return new ArrayTypeRepresentation(typeNode.ArrayType.Type.ToTypeRepresentation());
            }

            if (typeNode.Record != null)
            {
                return new RecordTypeRepresentation(
                    typeNode.Record.VariableDeclarations.ToDictionary(
                        node => node.Identifier.Lexeme,
                        node => node.ToTypeRepresentation()
                    )
                );
            }

            if (typeNode.Identifier != null)
            {
                return new TypeAliasRepresentation(typeNode.Identifier.Lexeme);
            }

            return new NoTypeRepresentation();
        }

        public static ITypeRepresentation ToTypeRepresentation(this ITypeNode typeNode) =>
            typeNode switch
            {
                PrimitiveTypeNode ptn => new PrimitiveTypeRepresentation(ptn),
                ArrayTypeNode atn => new ArrayTypeRepresentation(atn.Type.ToTypeRepresentation()),
                RecordTypeNode rtn => new RecordTypeRepresentation(
                        rtn.VariableDeclarations.ToDictionary(
                            node => node.Identifier.Lexeme,
                            node => node.ToTypeRepresentation()
                        )
                    ),
                _ => new NoTypeRepresentation(),
            };
    }
}