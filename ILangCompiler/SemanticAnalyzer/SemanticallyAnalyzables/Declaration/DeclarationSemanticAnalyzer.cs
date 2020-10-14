using System.Collections.Immutable;
using System.Linq;
using ILangCompiler.Parser.AST.Declarations;
using ILangCompiler.SemanticAnalyzer.Exceptions.Declaration;
using ILangCompiler.SemanticAnalyzer.Exceptions.Declaration.SimpleDeclaration;
using ILangCompiler.SemanticAnalyzer.SemanticallyAnalyzables.Declaration.RoutineDeclaration;
using ILangCompiler.SemanticAnalyzer.SemanticallyAnalyzables.Declaration.SimpleDeclaration;

namespace ILangCompiler.SemanticAnalyzer.SemanticallyAnalyzables.Declaration
{
    public struct DeclarationSemanticAnalyzer : ISemanticallyAnalyzable<IDeclarationNode, DeclarationSemanticException>
    {
        public ImmutableList<DeclarationSemanticException> AnalyzeSemantics(IDeclarationNode node) =>
            (node switch
            {
                ISimpleDeclarationNode sd => default(SimpleDeclarationSemanticAnalyzer).AnalyzeSemantics(sd)
                    .Select(e => (SimpleDeclarationSemanticException) e),
                RoutineDeclarationNode rd => default(RoutineDeclarationSemanticAnalyzer).AnalyzeSemantics(rd)
                    .Select(e => (DeclarationSemanticException) e),
                // _ => // TODO
            }).ToImmutableList();
    }
}