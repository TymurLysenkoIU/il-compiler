using System.Collections.Immutable;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.SemanticAnalyzer.Exceptions.Declaration.SimpleDeclaration.TypeDeclaration;
using ILangCompiler.SemanticAnalyzer.Exceptions.Declaration.SimpleDeclaration.VariableDeclaration;

namespace ILangCompiler.SemanticAnalyzer.SemanticallyAnalyzables.Declaration.SimpleDeclaration
{
    public struct VariableDeclarationSemanticAnalyzer : ISemanticallyAnalyzable<VariableDeclarationNode, VariableDeclarationSemanticException>
    {
        public ImmutableList<VariableDeclarationSemanticException> AnalyzeSemantics(VariableDeclarationNode node) =>
            ImmutableList<VariableDeclarationSemanticException>.Empty; // TODO: implement
    }
}