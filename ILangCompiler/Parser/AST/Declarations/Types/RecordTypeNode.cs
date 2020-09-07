using System.Collections.Immutable;

namespace ILangCompiler.Parser.AST.Declarations.Types
{
    public class RecordTypeNode : ITypeNode
    {
        public ImmutableArray<IAstNode> Fields; // TODO: change type to variable declaration
    }
}