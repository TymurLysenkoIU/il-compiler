using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using FunctionalExtensions.IO;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Literals;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords;
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
      uint lexemeStartPositionInLine = 1;
      var maybeCurrentChar = Option<int>.None;

      var currentLexemeBuffer = new StringBuilder();
      // TODO: fix this
      var absolutePosition = source.BaseStream.Position;

      var maybeToken = Option<IToken>.None;;

      while ((maybeCurrentChar = source.Read()).IsSome)
      {
        var currentChar = maybeCurrentChar.Value();

        maybeToken = Option<IToken>.None;

        switch (currentChar)
        {
          case var c when string.IsNullOrWhiteSpace(char.ConvertFromUtf32(c)):
            // if a whitespace was encountered - strip it
            // and yield whatever in the buffer to the output

            maybeToken = FlushBuffer(
              currentLexemeBuffer,
              (uint) absolutePosition,
              lineNumber,
              ref lexemeStartPositionInLine
            );
            if (maybeToken.IsSome)
              yield return maybeToken.ValueUnsafe();

            switch (c)
            {
              case '\r':
                yield return source.Read()
                  .Some<IToken>(cn =>
                    cn == '\n' ?
                      (IToken) new NewLineSymbolToken(
                        (uint) absolutePosition,
                        lineNumber,
                        lexemeStartPositionInLine
                      ) :
                      (IToken) new UnrecognizedToken(
                        $"\r{cn}",
                        (uint) absolutePosition,
                        lineNumber,
                        lexemeStartPositionInLine
                      )
                  )
                  .None(new UnrecognizedToken(
                    $"\r",
                    (uint) absolutePosition,
                    lineNumber,
                    lexemeStartPositionInLine
                  ))
                ;

                lineNumber += 1;
                lexemeStartPositionInLine = 1;

                break;
              case '\n':
                yield return new NewLineSymbolToken(
                  (uint) absolutePosition,
                  lineNumber,
                  lexemeStartPositionInLine
                );

                lineNumber += 1;
                lexemeStartPositionInLine = 1;

                break;
              default:
                lexemeStartPositionInLine += 1;
                break;
            }
            absolutePosition = source.BaseStream.Position;

            break;

          case '.':
            var currentLexeme = currentLexemeBuffer.ToString();

            var maybeBeforeToken =
              IntegerLiteralToken.FromString(
                currentLexeme,
                (uint) absolutePosition,
                lineNumber,
                lexemeStartPositionInLine
              ) ||
              IdentifierToken.FromString(
                currentLexeme,
                (uint) absolutePosition,
                lineNumber,
                lexemeStartPositionInLine
              ) ||
              UnrecognizedToken.FromString(
                currentLexeme,
                (uint) absolutePosition,
                lineNumber,
                lexemeStartPositionInLine
              )
            ;

            var tokes =
              source.Peek()
                .Some<ImmutableList<IToken>>(c =>
                {
                  var result = ImmutableList<IToken>.Empty;
                  IToken tokenToAdd = null;

                  switch (c)
                  {
                    case var _ when IsDigit(char.ConvertFromUtf32(c)):
                      currentLexemeBuffer.Append('.');
                      return ImmutableList<IToken>.Empty;
                    case '.':
                      tokenToAdd = new RangeSymbolToken(
                        (uint) absolutePosition,
                        lineNumber,
                        lexemeStartPositionInLine
                      );

                      result = maybeBeforeToken
                        .ToImmutableList()
                        .Add(tokenToAdd);
                      source.Read();
                      currentLexemeBuffer.Clear();
                      lexemeStartPositionInLine += maybeBeforeToken
                        .Some(t => (uint) t.Lexeme.Length)
                        .None(0u) + (uint) (tokenToAdd?.Lexeme.Length ?? 0);
                      absolutePosition = source.BaseStream.Position;

                      return result;

                    default:
                      tokenToAdd = new DotSymbolToken(
                        (uint) absolutePosition,
                        lineNumber,
                        lexemeStartPositionInLine
                      );

                      result = maybeBeforeToken
                        .ToImmutableList()
                        .Add(tokenToAdd);
                      currentLexemeBuffer.Clear();
                      lexemeStartPositionInLine += maybeBeforeToken
                        .Some(t => (uint) t.Lexeme.Length)
                        .None(0u) + (uint) (tokenToAdd?.Lexeme.Length ?? 0);
                      absolutePosition = source.BaseStream.Position;

                      return result;
                  }
                })
                .None(() =>
                {
                  var tokenToAdd = new DotSymbolToken(
                    (uint) absolutePosition,
                    lineNumber,
                    lexemeStartPositionInLine
                  );

                  var result = maybeBeforeToken
                    .ToImmutableList()
                    .Add(tokenToAdd);
                  currentLexemeBuffer.Clear();
                  lexemeStartPositionInLine += maybeBeforeToken
                    .Some(t => (uint) t.Lexeme.Length)
                    .None(0u) + (uint) (tokenToAdd?.Lexeme.Length ?? 0);
                  absolutePosition = source.BaseStream.Position;

                  return result;
                })
            ;

            foreach (var token in tokes)
            {
              yield return token;
            }

            break;

          case '/':
            maybeToken = FlushBuffer(
              currentLexemeBuffer,
              (uint) absolutePosition,
              lineNumber,
              ref lexemeStartPositionInLine
            );
            if (maybeToken.IsSome)
              yield return maybeToken.ValueUnsafe();
            absolutePosition = source.BaseStream.Position;

            yield return source.Peek()
              .Filter(c => c == '/')
              .Some<IToken>(c =>
              {
                var result = new CommentToken(
                  $"//{source.ReadLine()}",
                  (uint) absolutePosition,
                  lineNumber,
                  lexemeStartPositionInLine
                );

                absolutePosition = source.BaseStream.Position;
                lineNumber += 1;
                lexemeStartPositionInLine = 0;

                return result;
              })
              .None(() => new DivideOperatorToken(
                (uint) source.BaseStream.Position,
                lineNumber,
                lexemeStartPositionInLine
              ));

            lexemeStartPositionInLine += 1;

            break;

          case ':':
            maybeToken = FlushBuffer(
              currentLexemeBuffer,
              (uint) absolutePosition,
              lineNumber,
              ref lexemeStartPositionInLine
            );
            if (maybeToken.IsSome)
              yield return maybeToken.ValueUnsafe();
            absolutePosition = source.BaseStream.Position;

            yield return source.Peek()
              .Filter(c => c == '=')
              .Some<IToken>(c =>
              {
                var result = new AssignmentOperatorToken(
                  (uint) absolutePosition,
                  lineNumber,
                  lexemeStartPositionInLine
                );

                source.Read();
                absolutePosition = source.BaseStream.Position;
                lexemeStartPositionInLine += 1;

                return result;
              })
              .None(new ColonSymbolToken(
                (uint) absolutePosition,
                lineNumber,
                lexemeStartPositionInLine
              ));

            lexemeStartPositionInLine += 1;

            break;

          case '!':
            maybeToken = FlushBuffer(
              currentLexemeBuffer,
              (uint) absolutePosition,
              lineNumber,
              ref lexemeStartPositionInLine
            );
            if (maybeToken.IsSome)
              yield return maybeToken.ValueUnsafe();
            absolutePosition = source.BaseStream.Position;

            yield return source.Peek()
              .Filter(c => c != '=')
              .Some<IToken>(_ =>
              {
                var result = new NotEqualsOperatorToken(
                  (uint) absolutePosition,
                  lineNumber,
                  lexemeStartPositionInLine
                );

                source.Read();
                absolutePosition = source.BaseStream.Position;
                lexemeStartPositionInLine += 1;

                return result;
              })
              .None(new UnrecognizedToken(
                "!",
                (uint) absolutePosition,
                lineNumber,
                lexemeStartPositionInLine
              ));

            lexemeStartPositionInLine += 1;

            break;

          case '>':
            maybeToken = FlushBuffer(
              currentLexemeBuffer,
              (uint) absolutePosition,
              lineNumber,
              ref lexemeStartPositionInLine
            );
            if (maybeToken.IsSome)
              yield return maybeToken.ValueUnsafe();
            absolutePosition = source.BaseStream.Position;

            yield return source.Peek()
              .Filter(c => c != '=')
              .Some<IToken>(_ =>
              {
                var result = new GeOperatorToken(
                  (uint) absolutePosition,
                  lineNumber,
                  lexemeStartPositionInLine
                );

                source.Read();
                absolutePosition = source.BaseStream.Position;
                lexemeStartPositionInLine += 1;

                return result;
              })
              .None(new GtOperatorToken(
                (uint) absolutePosition,
                lineNumber,
                lexemeStartPositionInLine
              ));

            lexemeStartPositionInLine += 1;

            break;

          case '<':
            maybeToken = FlushBuffer(
              currentLexemeBuffer,
              (uint) absolutePosition,
              lineNumber,
              ref lexemeStartPositionInLine
            );
            if (maybeToken.IsSome)
              yield return maybeToken.ValueUnsafe();
            absolutePosition = source.BaseStream.Position;

            yield return source.Peek()
              .Filter(c => c != '=')
              .Some<IToken>(_ =>
              {
                var result = new LeOperatorToken(
                  (uint) absolutePosition,
                  lineNumber,
                  lexemeStartPositionInLine
                );

                source.Read();
                absolutePosition = source.BaseStream.Position;
                lexemeStartPositionInLine += 1;

                return result;
              })
              .None(new LtOperatorToken(
                (uint) absolutePosition,
                lineNumber,
                lexemeStartPositionInLine
              ));

            lexemeStartPositionInLine += 1;

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
          case ';':
            maybeToken = FlushBuffer(
              currentLexemeBuffer,
              (uint) absolutePosition,
              lineNumber,
              ref lexemeStartPositionInLine
            );
            if (maybeToken.IsSome)
              yield return maybeToken.ValueUnsafe();

            yield return SymbolLexemes
              .TryGetValue(((char) currentChar).ToString())
              .Some(cons => cons(
                (uint) absolutePosition,
                lineNumber,
                lexemeStartPositionInLine
              ))
              .None(() => new UnrecognizedToken(
                  currentChar.ToString(),
                  (uint) absolutePosition,
                  lineNumber,
                  lexemeStartPositionInLine
                )
              );

            lexemeStartPositionInLine += 1;

            break;

          default:
            if (currentLexemeBuffer.Length == 0)
              absolutePosition = source.BaseStream.Position;

            currentLexemeBuffer.Append(char.ConvertFromUtf32(currentChar));
            break;
        }
      }

      maybeToken = FlushBuffer(
        currentLexemeBuffer,
        (uint) absolutePosition,
        lineNumber,
        ref lexemeStartPositionInLine
      );
      if (maybeToken.IsSome)
        yield return maybeToken.ValueUnsafe();
    }

    private Option<IToken> FlushBuffer(
      StringBuilder buffer,
      uint absolutePosition,
      uint lineNumber,
      ref uint lexemeStartPositionInLine
    )
    {
      if (buffer.Length > 0)
      {
        var lexeme = buffer.ToString();

        var result =
          KeywordToken.FromString(
            lexeme,
            absolutePosition,
            lineNumber,
            lexemeStartPositionInLine
          ) ||
          IdentifierToken.FromString(
            lexeme,
            absolutePosition,
            lineNumber,
            lexemeStartPositionInLine
          ) ||
          IntegerLiteralToken.FromString(
            lexeme,
            absolutePosition,
            lineNumber,
            lexemeStartPositionInLine
          ) ||
          RealLiteralToken.FromString(
            lexeme,
            absolutePosition,
            lineNumber,
            lexemeStartPositionInLine
          ) ||
          new UnrecognizedToken(
            lexeme,
            absolutePosition,
            lineNumber,
            lexemeStartPositionInLine
          )
        ;

        buffer.Clear();

        lexemeStartPositionInLine += (uint) lexeme.Length;

        return result;
      }
      else
      {
        return Option<IToken>.None;
      }
    }

    private bool IsDigit(string str) =>
      str == "0" || str == "1" || str == "2" ||
      str == "3" || str == "4" || str == "5" ||
      str == "6" || str == "7" || str == "8" ||
                    str == "9"
    ;

    public static IReadOnlyDictionary<
      string,
      Func<uint, uint, uint, IToken>
    > SymbolLexemes { get; } =
      new Dictionary<string, Func<uint, uint, uint, IToken>>
      {
        // #region Keywords
        //
        // #region Control statements
        //
        // [ElseKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new ElseKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        // [ForKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new ForKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        // [IfKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new IfKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        // [LoopKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new LoopKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        // [ThenKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new ThenKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        // [WhileKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new WhileKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        //
        // #endregion
        //
        // #region Declaration-related
        //
        // [ArrayKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new ArrayKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        // [IsKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new IsKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        // [RecordKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new RecordKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        // [RoutineKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new RoutineKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        // [TypeKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new TypeKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        // [VarKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new VarKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        //
        // #endregion
        //
        // #region Boolean operations
        //
        // [AndKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new AndKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        // [NotKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new NotKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        // [OrKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new OrKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        // [XorKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new XorKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        //
        // #endregion
        //
        // #region Primitive types
        //
        // [BooleanKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new BooleanKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        // [IntegerKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new IntegerKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        // [RealKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new RealKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        //
        // #endregion
        //
        // #region Other
        //
        // [EndKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new EndKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        // [FalseKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new FalseKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        // [TrueKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new TrueKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        // [ReverseKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new ReverseKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        // [InKeywordToken.LexemeValue] =
        //   (ap, ln, pl) =>
        //     new InKeywordToken(
        //       ap,
        //       ln,
        //       pl
        //     ),
        //
        // #endregion
        //
        // #endregion

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
        [SemicolonSymbolToken.LexemeValue] =
          (ap, ln, pl) =>
            new SemicolonSymbolToken(
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
