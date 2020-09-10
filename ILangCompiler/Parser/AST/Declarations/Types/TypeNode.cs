﻿using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.AST.Declarations.Types.PrimitiveTypes;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Declarations.Types
{
    public class TypeNode:ITypeNode
    {
        private TypeNode()
        {
            
        }

        private static ParseException NotATypeException => new ParseException("Not a type");

        
        public static Either<ParseException, Pair<List<IToken>, TypeNode>> Parse(List<IToken> tokens)
        {
            Console.WriteLine("TypeNode");
            var maybePrimitiveType = PrimitiveTypeNode.Parse(tokens);
            if (maybePrimitiveType.IsLeft)
            {
                var maybeArrayType = ArrayTypeNode.Parse(tokens);
                if (maybeArrayType.IsLeft)
                {
                    var maybeRecordType = RecordTypeNode.Parse(tokens);
                    if (maybeRecordType.IsLeft)
                    {
                        if (tokens.Count < 1)
                            return NotATypeException;
                        if (!(tokens[0] is IdentifierToken))
                            return NotATypeException;
                        tokens = tokens.Skip(1).ToList();
                        return new Pair<List<IToken>, TypeNode> (tokens, new TypeNode());

                    }
                    tokens = maybeRecordType.RightToList()[0].First;
                    return new Pair<List<IToken>, TypeNode> (tokens, new TypeNode());
                }
                tokens = maybeArrayType.RightToList()[0].First;
                return new Pair<List<IToken>, TypeNode> (tokens, new TypeNode());
            }

            tokens = maybePrimitiveType.RightToList()[0].First;
            return new Pair<List<IToken>, TypeNode> (tokens, new TypeNode());
             
        }
    }
}