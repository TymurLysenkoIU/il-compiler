using System.Collections.Immutable;
using System.Linq;
using ILangCompiler.Parser.AST;
using ILangCompiler.SemanticAnalyzer.Exceptions;
using ILangCompiler.SemanticAnalyzer.SemanticallyAnalyzables.Declaration;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ILangCompiler.SemanticAnalyzer.SemanticallyAnalyzables
{
    public struct ProgramNodeSemanticAnalyzer : ISemanticallyAnalyzable<ProgramNode, SemanticException>
    {
        public ImmutableList<SemanticException> AnalyzeSemantics(ProgramNode node) =>
            node.Declarations
                .SelectMany(decl =>
                    default(DeclarationSemanticAnalyzer).AnalyzeSemantics(decl)
                        .Select(e => (SemanticException) e)
                )
                .ToImmutableList();
    }
}