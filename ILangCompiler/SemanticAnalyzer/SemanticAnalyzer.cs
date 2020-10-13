using System.Collections.Immutable;
using ILangCompiler.Parser.AST;
using ILangCompiler.SemanticAnalyzer.Exceptions;
using ILangCompiler.SemanticAnalyzer.SemanticallyAnalyzables;

namespace ILangCompiler.SemanticAnalyzer
{
    public class SemanticAnalyzer
    {
        public ImmutableList<SemanticException> Analyze(ProgramNode programNode) =>
            default(ProgramNodeSemanticAnalyzer).AnalyzeSemantics(programNode);
    }
}
