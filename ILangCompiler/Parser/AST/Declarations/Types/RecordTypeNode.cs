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

        private RecordTypeNode()
        {
            
        }

        public static Either<ParseException, RecordTypeNode> Parse(List<IToken> tokens)
        {
            if (tokens.Count < 1)
                return NotARecordTypeException;
            if (!(tokens[0] is RecordKeywordToken))
                return NotARecordTypeException;
            tokens.Skip(1).ToList();
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken )
                    tokens.Skip(1).ToList();
                else break;
            
            while (true)
            {
                var maybeVariableDeclaration = VariableDeclarationNode.Parse(tokens);
                if (maybeVariableDeclaration.IsLeft)
                    break;
            }
            
            if (tokens.Count < 1)
                return NotARecordTypeException;
            if (!(tokens[0] is EndKeywordToken))
                return NotARecordTypeException;
            tokens.Skip(1).ToList();
            while (tokens.Count > 0)
                if (tokens[0] is NewLineSymbolToken || tokens[0] is CommentToken ||
                    tokens[0] is SemicolonSymbolToken)
                    tokens.Skip(1).ToList();
                else break;
            
            return new RecordTypeNode();

        }


    }
}