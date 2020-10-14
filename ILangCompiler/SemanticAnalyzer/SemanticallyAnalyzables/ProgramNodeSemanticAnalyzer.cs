using System.Collections.Immutable;
using System.Linq;
using ILangCompiler.Parser.AST;
using ILangCompiler.Parser.AST.Declarations;
using ILangCompiler.Parser.AST.Declarations.Types;
using ILangCompiler.SemanticAnalyzer.Exceptions;
using ILangCompiler.SemanticAnalyzer.Exceptions.Declaration;
using ILangCompiler.SemanticAnalyzer.SemanticallyAnalyzables.Declaration;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ILangCompiler.SemanticAnalyzer.SemanticallyAnalyzables
{
    public struct ProgramNodeSemanticAnalyzer : ISemanticallyAnalyzable<ProgramNode, DeclarationSemanticException>
    {
        public ImmutableList<DeclarationSemanticException> AnalyzeSemantics(ProgramNode node) =>
            node.Declarations
            .GroupBy(
                decl => decl switch
                {
                    TypeDeclarationNode td => td.Identifier.Lexeme,
                    VariableDeclarationNode vd => vd.Identifier.Lexeme,
                    RoutineDeclarationNode rd => rd.Identifier.Lexeme,
                    _ => "",
                }
            )
            .Filter(decls => !string.IsNullOrEmpty(decls.Key))
            .Fold(
                ImmutableList<DeclarationSemanticException>.Empty,
                (errors, decls) =>
                    decls.Count() > 1 ?
                        errors.Add(
                            new DeclarationSemanticException(
                                decls.First(),
                                $"Redeclaration of a name in the same scope {decls.Key}"
                            )
                        ) :
                        errors
            )
            .Concat(
                node.Declarations
                    .SelectMany(decl =>
                        default(DeclarationSemanticAnalyzer).AnalyzeSemantics(decl)
                    )
            )
            .ToImmutableList();
    }
}