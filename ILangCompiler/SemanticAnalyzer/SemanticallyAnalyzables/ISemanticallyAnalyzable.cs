using System.Collections.Immutable;
using ILangCompiler.Parser.AST;
using ILangCompiler.SemanticAnalyzer.Exceptions;
using LanguageExt;

namespace ILangCompiler.SemanticAnalyzer.SemanticallyAnalyzables
{
    public interface ISemanticallyAnalyzable<T, TE>
        where T : IAstNode
        where TE: SemanticException
    {
        public ImmutableList<TE> AnalyzeSemantics(T node);
    }
}