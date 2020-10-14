using System;
using System.Collections.Generic;
using System.Linq;
using ILangCompiler.Parser.AST.Declarations.Types.PrimitiveTypes;
using ILangCompiler.Parser.AST.TypeTable;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Declarations.Types
{
    public class TypeNode:ITypeNode
    {
        public PrimitiveTypeNode PrimitiveType;
        public ArrayTypeNode ArrayType;
        public RecordTypeNode Record;
        public IdentifierToken Identifier;
        public TypeNode(PrimitiveTypeNode primitiveType)
        {
            PrimitiveType = primitiveType;
        }
        public TypeNode(ArrayTypeNode arrayType)
        {
            ArrayType = arrayType;
        }
        public TypeNode(RecordTypeNode record)
        {
            Record = record;
        }
        public TypeNode(IdentifierToken identifier)
        {
            Identifier = identifier;
        }

        private static ParseException NotATypeException => new ParseException("Not a type");

        public static Either<ParseException, Pair<List<IToken>, TypeNode>> Parse(List<IToken> tokens, SymT symT, IScopedTable<IEntityType, string> parentTypeTable)
        {
            Console.WriteLine("TypeNode");
            var maybePrimitiveType = PrimitiveTypeNode.Parse(tokens);
            if (maybePrimitiveType.IsLeft)
            {
                var maybeArrayType = ArrayTypeNode.Parse(tokens, symT, parentTypeTable);
                if (maybeArrayType.IsLeft)
                {
                    SymT NewSymT = new SymT(symT);
                    var maybeRecordType = RecordTypeNode.Parse(tokens, NewSymT, parentTypeTable);

                    if (maybeRecordType.IsLeft)
                    {
                        if (tokens.Count < 1)
                            return NotATypeException;
                        if (!(tokens[0] is IdentifierToken))
                            return NotATypeException;
                        IdentifierToken identifier = (IdentifierToken) tokens[0];
                        if (!(symT.ContainRec(identifier)))
                        {
                            // TODO: return an exception instead
                            Console.Write("No such identifier");
                        }
                        tokens = tokens.Skip(1).ToList();
                        return new Pair<List<IToken>, TypeNode> (tokens, new TypeNode(identifier));

                    }
                    tokens = maybeRecordType.RightToList()[0].First;
                    return new Pair<List<IToken>, TypeNode> (tokens, new TypeNode(
                        maybeRecordType.RightToList()[0].Second));
                }
                tokens = maybeArrayType.RightToList()[0].First;
                return new Pair<List<IToken>, TypeNode> (tokens, new TypeNode(
                    maybeArrayType.RightToList()[0].Second));
            }

            tokens = maybePrimitiveType.RightToList()[0].First;
            return new Pair<List<IToken>, TypeNode> (tokens, new TypeNode(
                maybePrimitiveType.RightToList()[0].Second));

        }
    }
}