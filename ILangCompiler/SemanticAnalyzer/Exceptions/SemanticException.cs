using System;
using ILangCompiler.Parser.AST;

namespace ILangCompiler.SemanticAnalyzer.Exceptions
{
    public abstract class SemanticException : Exception
    {
        public SemanticException(IAstNode node, string message) : base(message)
        {
        }
    }
}