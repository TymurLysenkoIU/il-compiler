using System.Collections.Immutable;
using System.Linq;
using ILangCompiler.Parser.AST.Declarations;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.SemanticAnalyzer.Exceptions.Declaration.SimpleDeclaration;

namespace ILangCompiler.SemanticAnalyzer.SemanticallyAnalyzables.Declaration.SimpleDeclaration
{
    public struct SimpleDeclarationSemanticAnalyzer : ISemanticallyAnalyzable<ISimpleDeclarationNode, SimpleDeclarationSemanticException>
    {
        public ImmutableList<SimpleDeclarationSemanticException> AnalyzeSemantics(ISimpleDeclarationNode node) =>
            (node switch
            {
                TypeDeclarationNode td => default(TypeDeclarationSemanticAnalyzer).AnalyzeSemantics(td)
                    .Select(e => (SimpleDeclarationSemanticException) e),
                VariableDeclarationNode vd => default(VariableDeclarationSemanticAnalyzer).AnalyzeSemantics(vd)
                    .Select(e => (SimpleDeclarationSemanticException) e),
                // _ => // TODO
            }).ToImmutableList();
    }
}