using System.Collections.Immutable;
using ILangCompiler.Parser.AST.Declarations;
using ILangCompiler.SemanticAnalyzer.Exceptions.Declaration.RoutineDeclaration;

namespace ILangCompiler.SemanticAnalyzer.SemanticallyAnalyzables.Declaration.RoutineDeclaration
{
    public struct RoutineDeclarationSemanticAnalyzer : ISemanticallyAnalyzable<RoutineDeclarationNode, RoutineDeclarationSemanticException>
    {
        public ImmutableList<RoutineDeclarationSemanticException> AnalyzeSemantics(RoutineDeclarationNode node) =>
            ImmutableList<RoutineDeclarationSemanticException>.Empty; // TODO
    }
}