using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ILangCompiler.Parser.Exceptions;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler.Parser.AST.Declarations.Types
{
    public class RecordTypeNode : ITypeNode
    {
        private static ParseException NotARecordTypeException => new ParseException("Not a record type");

        public List<VariableDeclarationNode> VariableDeclarations;
        public SymT SymbolTable;
        private RecordTypeNode(List<VariableDeclarationNode> varDeclNodes, SymT symT)
        {
            VariableDeclarations = varDeclNodes;
            SymbolTable = symT;
        }

        public static Either<ParseException, Pair<List<IToken>, RecordTypeNode>> Parse(List<IToken> tokens, SymT symT)
        {
            Console.WriteLine("RecordTypeNode");
            var declarations = new List<VariableDeclarationNode>();
            if (tokens.Count < 1)
                return NotARecordTypeException;
            if (!(tokens[0] is RecordKeywordToken))
                return NotARecordTypeException;
            tokens = tokens.Skip(1).ToList();
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken )
                    tokens = tokens.Skip(1).ToList();
                else break;
            
            while (true)
            {
                var maybeVariableDeclaration = VariableDeclarationNode.Parse(tokens, symT);
                if (maybeVariableDeclaration.IsLeft)
                    break;
                declarations.Add(maybeVariableDeclaration.RightToList()[0].Second);
                tokens = maybeVariableDeclaration.RightToList()[0].First;
            }
            
            if (tokens.Count < 1)
                return NotARecordTypeException;
            if (!(tokens[0] is EndKeywordToken))
                return NotARecordTypeException;
            tokens = tokens.Skip(1).ToList();
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                    tokens[0] is SemicolonSymbolToken)
                    tokens = tokens.Skip(1).ToList();
                else break;
            
            return new Pair<List<IToken>, RecordTypeNode>(tokens, new RecordTypeNode(declarations, symT));

        }


    }
}