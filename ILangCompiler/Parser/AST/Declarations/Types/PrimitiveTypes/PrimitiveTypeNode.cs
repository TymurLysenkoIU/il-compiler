using System;
using System.Collections.Generic;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Declarations.Types.PrimitiveTypes
{
    public class PrimitiveTypeNode
    {
        private static ParseException NotAPrimitiveTypeException => new ParseException("Not a primitive type");
        private PrimitiveTypeNode()
        {
        }

        public static Either<ParseException, Pair <List<IToken>, PrimitiveTypeNode>> Parse(List<IToken> tokens)
        {
            Console.WriteLine("PrimitiveTypeNode");
            var maybeInteger = IntegerTypeNode.Parse(tokens);
            if (maybeInteger.IsLeft)
            {
                var maybeReal = RealTypeNode.Parse(tokens);
                if (maybeReal.IsLeft)
                {
                    var maybeBoolean = BooleanTypeNode.Parse(tokens);
                    if (maybeBoolean.IsLeft)
                        return NotAPrimitiveTypeException;

                    tokens = maybeBoolean.RightToList()[0].First;
                    return new Pair <List<IToken>, PrimitiveTypeNode>(tokens, new PrimitiveTypeNode());
                    
                }
                tokens = maybeReal.RightToList()[0].First;
                return new Pair <List<IToken>, PrimitiveTypeNode>(tokens, new PrimitiveTypeNode());

            }
            tokens = maybeInteger.RightToList()[0].First;
            return new Pair <List<IToken>, PrimitiveTypeNode>(tokens, new PrimitiveTypeNode());

        }
    }
}