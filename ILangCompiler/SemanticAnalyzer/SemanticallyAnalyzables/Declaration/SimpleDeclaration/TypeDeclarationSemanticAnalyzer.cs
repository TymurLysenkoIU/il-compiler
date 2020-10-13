using System.Collections.Immutable;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.SemanticAnalyzer.Exceptions.Declaration.SimpleDeclaration.TypeDeclaration;

namespace ILangCompiler.SemanticAnalyzer.SemanticallyAnalyzables.Declaration.SimpleDeclaration
{
    public struct TypeDeclarationSemanticAnalyzer : ISemanticallyAnalyzable<TypeDeclarationNode, TypeDeclarationSemanticException>
    {
        public ImmutableList<TypeDeclarationSemanticException> AnalyzeSemantics(TypeDeclarationNode node) =>
            ImmutableList<TypeDeclarationSemanticException>.Empty; // TODO: implement
    }
}