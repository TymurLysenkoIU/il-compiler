using System;
using System.Collections.Generic;
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
        private RecordTypeNode(List<VariableDeclarationNode> var_decl_nodes)
        {
            VariableDeclarations = var_decl_nodes;
        }

        public static Either<ParseException, Pair<List<IToken>, RecordTypeNode>> Parse(List<IToken> tokens)
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
                var maybeVariableDeclaration = VariableDeclarationNode.Parse(tokens);
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
            
            return new Pair<List<IToken>, RecordTypeNode>(tokens, new RecordTypeNode(declarations));

        }


    }
}