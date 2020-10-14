using System;
using System.Collections.Generic;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Declarations.Types.PrimitiveTypes
{
    public abstract class PrimitiveTypeNode
    {
        private static ParseException NotAPrimitiveTypeException => new ParseException("Not a primitive type");

        protected PrimitiveTypeNode()
        {
        }

        public static Either<ParseException, Pair <List<IToken>, PrimitiveTypeNode>> Parse(List<IToken> tokens)
        {
            Console.WriteLine("PrimitiveTypeNode");
            return
                IntegerTypeNode.Parse(tokens)
                    .Map(p => new Pair<List<IToken>, PrimitiveTypeNode>(p.First, p.Second)) ||
                RealTypeNode.Parse(tokens)
                    .Map(p => new Pair<List<IToken>, PrimitiveTypeNode>(p.First, p.Second)) ||
                BooleanTypeNode.Parse(tokens)
                    .Map(p => new Pair<List<IToken>, PrimitiveTypeNode>(p.First, p.Second))
            ;
        }
    }
}