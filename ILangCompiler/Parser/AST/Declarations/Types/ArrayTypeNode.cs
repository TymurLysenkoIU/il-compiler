using System;

namespace ILangCompiler.Parser.AST.Declarations.Types
{
    public class ArrayTypeNode : ITypeNode
    {
        public IAstNode InitializerExpression { get; } // TODO: must have expression type
        public Type ArrayType { get; }
    }
}