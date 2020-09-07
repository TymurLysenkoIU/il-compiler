using System.Collections.Immutable;

namespace ILangCompiler.Parser.AST
{
    public class BodyNode : IAstNode
    {
        public ImmutableArray<IBodyElementNode> Elements { get; }
    }
}