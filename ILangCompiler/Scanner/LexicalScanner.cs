using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FunctionalExtensions.IO;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Predefined;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.Constructions;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.LogicalOperations;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.PrimitiveTypes;
using ILangCompiler.Scanner.Tokens.Predefined.Operators;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ILangCompiler.Scanner
{
  public class LexicalScanner
  {
    public IEnumerable<IToken> Tokenize(SafeStreamReader source)
    {
      uint lineNumber = 1;
      uint currentPositionInLine = 0;
      var maybeCurrentChar = Option<int>.None;

      var currentTokenBuilder = new StringBuilder();
      var absolutePosition = source.BaseStream.Position;

      while ((maybeCurrentChar = source.Read()).IsSome)
      {
        var currentChar = maybeCurrentChar.Value();

        switch (currentChar)
        {
          case var c when string.IsNullOrWhiteSpace(char.ConvertFromUtf32(c)):
            // if a whitespace was encountered - strip it
            // and yield whatever in the buffer to the output

            if (c == '\n')
            {
              lineNumber += 1;
              currentPositionInLine = 0;
            }
            else if (c != '\r')
            {
              currentPositionInLine += 1;
            }

            // TODO: produce a token from the buffer
            // TODO: the buffer contains either a predefined, literal, identifier or an unrecognized token
            if (currentTokenBuilder.Length > 0)
              yield return new UnrecognizedToken(
                currentTokenBuilder.ToString(),
                (uint) absolutePosition,
                lineNumber,
                currentPositionInLine
              );

            currentTokenBuilder.Clear();

            break;

          case '/':
            yield return source.Peek()
              .Filter(c => c == '/')
              .Some<IToken>(c =>
              {
                var result = new CommentToken(
                  $"//{source.ReadLine()}",
                  (uint) source.BaseStream.Position,
                  lineNumber,
                  currentPositionInLine
                );

                lineNumber += 1;
                currentPositionInLine = 0;
                currentTokenBuilder.Clear();

                return result;
              })
              .None(() =>
              {
                currentTokenBuilder.Clear();

                return new DivideOperatorToken(
                  (uint) source.BaseStream.Position,
                  lineNumber,
                  currentPositionInLine
                );
              });
            break;

          case ':':
            yield return source.Peek()
              .Filter(c => c == '=')
              .Some<IToken>(c =>
              {
                currentTokenBuilder.Clear();
                currentPositionInLine += 1;
                source.Read();

                return new AssignmentOperatorToken(
                  (uint) absolutePosition,
                  lineNumber,
                  currentPositionInLine
                );
              })
              .None(() =>
              {
                currentTokenBuilder.Clear();

                return new ColonSymbolToken(
                  (uint) absolutePosition,
                  lineNumber,
                  currentPositionInLine
                );
              });
            ;
            break;

          case '*':
          case '%':
          case '+':
          case '-':
          case '=':
          case ',':
          case '[':
          case ']':
          case '(':
          case ')':
            // TODO: produce a token from the buffer
            if (currentTokenBuilder.Length > 0)
              yield return new UnrecognizedToken(
                currentTokenBuilder.ToString(),
                (uint) absolutePosition,
                lineNumber,
                currentPositionInLine
              );

            currentTokenBuilder.Clear();

            yield return PredefinedLexemes
              .TryGetValue(((char) currentChar).ToString())
              .Some(cons => cons(
                (uint) absolutePosition,
                lineNumber,
                currentPositionInLine
              ))
              .None(() => new UnrecognizedToken(
                currentChar.ToString(),
                (uint) absolutePosition,
                lineNumber,
                currentPositionInLine
              )
            );

            break;

          // TODO: !=, >=, >, <=, <
          // TODO: '.', ..

          default:
            if (currentTokenBuilder.Length == 0)
              absolutePosition = source.BaseStream.Position;

            currentTokenBuilder.Append(char.ConvertFromUtf32(currentChar));
            break;
        }
      }

      // TODO: produce a token from the buffer
      if (currentTokenBuilder.Length > 0)
        yield return new UnrecognizedToken(
          currentTokenBuilder.ToString(),
          (uint) absolutePosition,
          lineNumber,
          currentPositionInLine
        );
    }

    public static IReadOnlyDictionary<
      string,
      Func<uint, uint, uint, IToken>
    > PredefinedLexemes { get; } =
      new Dictionary<string, Func<uint, uint, uint, IToken>>
      {
        #region Keywords

        #region Control statements

        [ElseKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new ElseKeywordToken(
              ap,
              ln,
              pl
            ),
        [ForKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new ForKeywordToken(
              ap,
              ln,
              pl
            ),
        [IfKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new IfKeywordToken(
              ap,
              ln,
              pl
            ),
        [LoopKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new LoopKeywordToken(
              ap,
              ln,
              pl
            ),
        [ThenKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new ThenKeywordToken(
              ap,
              ln,
              pl
            ),
        [WhileKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new WhileKeywordToken(
              ap,
              ln,
              pl
            ),

        #endregion

        #region Declaration-related

        [ArrayKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new ArrayKeywordToken(
              ap,
              ln,
              pl
            ),
        [IsKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new IsKeywordToken(
              ap,
              ln,
              pl
            ),
        [RecordKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new RecordKeywordToken(
              ap,
              ln,
              pl
            ),
        [RoutineKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new RoutineKeywordToken(
              ap,
              ln,
              pl
            ),
        [TypeKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new TypeKeywordToken(
              ap,
              ln,
              pl
            ),
        [VarKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new VarKeywordToken(
              ap,
              ln,
              pl
            ),

        #endregion

        #region Boolean operations

        [AndKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new AndKeywordToken(
              ap,
              ln,
              pl
            ),
        [NotKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new NotKeywordToken(
              ap,
              ln,
              pl
            ),
        [OrKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new OrKeywordToken(
              ap,
              ln,
              pl
            ),
        [XorKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new XorKeywordToken(
              ap,
              ln,
              pl
            ),

        #endregion

        #region Primitive types

        [BooleanKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new BooleanKeywordToken(
              ap,
              ln,
              pl
            ),
        [IntegerKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new IntegerKeywordToken(
              ap,
              ln,
              pl
            ),
        [RealKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new RealKeywordToken(
              ap,
              ln,
              pl
            ),

        #endregion

        #region Other

        [EndKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new EndKeywordToken(
              ap,
              ln,
              pl
            ),
        [FalseKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new FalseKeywordToken(
              ap,
              ln,
              pl
            ),
        [TrueKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new TrueKeywordToken(
              ap,
              ln,
              pl
            ),
        [ReverseKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new ReverseKeywordToken(
              ap,
              ln,
              pl
            ),
        [InKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new InKeywordToken(
              ap,
              ln,
              pl
            ),

        #endregion

        #endregion

        #region Operators

        #region Assignment

        [AssignmentOperatorToken.LexemeValue] =
          (ap, ln, pl) =>
            new AssignmentOperatorToken(
              ap,
              ln,
              pl
            ),

        #endregion

        #region Numerical with high precedence

        [DivideOperatorToken.LexemeValue] =
          (ap, ln, pl) =>
            new DivideOperatorToken(
              ap,
              ln,
              pl
            ),
        [MultiplyOperatorToken.LexemeValue] =
          (ap, ln, pl) =>
            new MultiplyOperatorToken(
              ap,
              ln,
              pl
            ),
        [ModOperatorToken.LexemeValue] =
          (ap, ln, pl) =>
            new ModOperatorToken(
              ap,
              ln,
              pl
            ),

        #endregion

        #region Numerical with low precedence

        [PlusOperatorToken.LexemeValue] =
          (ap, ln, pl) =>
            new PlusOperatorToken(
              ap,
              ln,
              pl
            ),
        [MinusOperatorToken.LexemeValue] =
          (ap, ln, pl) =>
            new MinusOperatorToken(
              ap,
              ln,
              pl
            ),

        #endregion

        #region Relational

        [EqualsOperatorToken.LexemeValue] =
          (ap, ln, pl) =>
            new EqualsOperatorToken(
              ap,
              ln,
              pl
            ),
        [NotEqualsOperatorToken.LexemeValue] =
          (ap, ln, pl) =>
            new NotEqualsOperatorToken(
              ap,
              ln,
              pl
            ),
        [GeOperatorToken.LexemeValue] =
          (ap, ln, pl) =>
            new GeOperatorToken(
              ap,
              ln,
              pl
            ),
        [GtOperatorToken.LexemeValue] =
          (ap, ln, pl) =>
            new GtOperatorToken(
              ap,
              ln,
              pl
            ),
        [LeOperatorToken.LexemeValue] =
          (ap, ln, pl) =>
            new LeOperatorToken(
              ap,
              ln,
              pl
            ),
        [LtOperatorToken.LexemeValue] =
          (ap, ln, pl) =>
            new LtOperatorToken(
              ap,
              ln,
              pl
            ),

        #endregion

        #region Symbols

        [ColonSymbolToken.LexemeValue] =
          (ap, ln, pl) =>
            new ColonSymbolToken(
              ap,
              ln,
              pl
            ),
        [ComaSymbolToken.LexemeValue] =
          (ap, ln, pl) =>
            new ComaSymbolToken(
              ap,
              ln,
              pl
            ),
        [DotSymbolToken.LexemeValue] =
          (ap, ln, pl) =>
            new DotSymbolToken(
              ap,
              ln,
              pl
            ),
        [LeftBracketSymbolToken.LexemeValue] =
          (ap, ln, pl) =>
            new LeftBracketSymbolToken(
              ap,
              ln,
              pl
            ),
        [RightBracketSymbolToken.LexemeValue] =
          (ap, ln, pl) =>
            new RightBracketSymbolToken(
              ap,
              ln,
              pl
            ),
        [LeftParenthSymbolToken.LexemeValue] =
          (ap, ln, pl) =>
            new LeftParenthSymbolToken(
              ap,
              ln,
              pl
            ),
        [RightParenthSymbolToken.LexemeValue] =
          (ap, ln, pl) =>
            new RightParenthSymbolToken(
              ap,
              ln,
              pl
            ),
        [RangeSymbolToken.LexemeValue] =
          (ap, ln, pl) =>
            new RangeSymbolToken(
              ap,
              ln,
              pl
            ),

        #endregion

        #endregion
      };
  }
}