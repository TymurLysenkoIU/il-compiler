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
      uint absolutePosition = 1;
      var maybeCurrentChar = Option<int>.None;

      var currentLexemeBuffer = new StringBuilder();

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
              ref absolutePosition,
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
                        absolutePosition,
                        lineNumber,
                        lexemeStartPositionInLine
                      ) :
                      (IToken) new UnrecognizedToken(
                        $"\r{cn}",
                        absolutePosition,
                        lineNumber,
                        lexemeStartPositionInLine
                      )
                  )
                  .None(new UnrecognizedToken(
                    $"\r",
                    absolutePosition,
                    lineNumber,
                    lexemeStartPositionInLine
                  ))
                ;

                absolutePosition += 2;
                lineNumber += 1;
                lexemeStartPositionInLine = 1;

                break;
              case '\n':
                yield return new NewLineSymbolToken(
                  absolutePosition,
                  lineNumber,
                  lexemeStartPositionInLine
                );

                absolutePosition += 1;
                lineNumber += 1;
                lexemeStartPositionInLine = 1;

                break;
              default:
                absolutePosition += 1;
                lexemeStartPositionInLine += 1;
                break;
            }

            break;

          case '.':
            var currentLexeme = currentLexemeBuffer.ToString();

            var maybeBeforeToken =
              IntegerLiteralToken.FromString(
                currentLexeme,
                absolutePosition,
                lineNumber,
                lexemeStartPositionInLine
              ) ||
              IdentifierToken.FromString(
                currentLexeme,
                absolutePosition,
                lineNumber,
                lexemeStartPositionInLine
              ) ||
              UnrecognizedToken.FromString(
                currentLexeme,
                absolutePosition,
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
                      absolutePosition += maybeBeforeToken
                        .Map(t => (uint) t.Lexeme.Length)
                        .IfNone(0);
                      lexemeStartPositionInLine += maybeBeforeToken
                        .Some(t => (uint) t.Lexeme.Length)
                        .None(0u);

                      tokenToAdd = new RangeSymbolToken(
                        absolutePosition,
                        lineNumber,
                        lexemeStartPositionInLine
                      );

                      result = maybeBeforeToken
                        .ToImmutableList()
                        .Add(tokenToAdd);
                      source.Read();
                      currentLexemeBuffer.Clear();
                      lexemeStartPositionInLine += (uint) (tokenToAdd?.Lexeme.Length ?? 0);
                      absolutePosition += (uint) (tokenToAdd?.Lexeme.Length ?? 0);

                      return result;

                    default:
                      absolutePosition += maybeBeforeToken
                        .Map(t => (uint) t.Lexeme.Length)
                        .IfNone(0);
                      lexemeStartPositionInLine += maybeBeforeToken
                        .Some(t => (uint) t.Lexeme.Length)
                        .None(0u);

                      tokenToAdd = new DotSymbolToken(
                        absolutePosition,
                        lineNumber,
                        lexemeStartPositionInLine
                      );

                      result = maybeBeforeToken
                        .ToImmutableList()
                        .Add(tokenToAdd);
                      currentLexemeBuffer.Clear();
                      lexemeStartPositionInLine += (uint) (tokenToAdd?.Lexeme.Length ?? 0);
                      absolutePosition += (uint) (tokenToAdd?.Lexeme.Length ?? 0);

                      return result;
                  }
                })
                .None(() =>
                {
                  absolutePosition += maybeBeforeToken
                    .Map(t => (uint) t.Lexeme.Length)
                    .IfNone(0);
                  lexemeStartPositionInLine += maybeBeforeToken
                    .Some(t => (uint) t.Lexeme.Length)
                    .None(0u);

                  var tokenToAdd = new DotSymbolToken(
                    absolutePosition,
                    lineNumber,
                    lexemeStartPositionInLine
                  );

                  var result = maybeBeforeToken
                    .ToImmutableList()
                    .Add(tokenToAdd);
                  currentLexemeBuffer.Clear();
                  lexemeStartPositionInLine += (uint) (tokenToAdd?.Lexeme.Length ?? 0);
                  absolutePosition += (uint) (tokenToAdd?.Lexeme.Length ?? 0);

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
              ref absolutePosition,
              lineNumber,
              ref lexemeStartPositionInLine
            );
            if (maybeToken.IsSome)
              yield return maybeToken.ValueUnsafe();

            yield return source.Peek()
              .Filter(c => c == '/')
              .Some<IToken>(c =>
              {
                var commentContent = source.ReadLine();

                var result = new CommentToken(
                  $"/{commentContent}",
                  absolutePosition,
                  lineNumber,
                  lexemeStartPositionInLine
                );

                absolutePosition += (uint) commentContent.Length;
                lineNumber += 1;
                lexemeStartPositionInLine = 0;

                return result;
              })
              .None(() => new DivideOperatorToken(
                (uint) source.BaseStream.Position,
                lineNumber,
                lexemeStartPositionInLine
              ));

            absolutePosition += 1;
            lexemeStartPositionInLine += 1;

            break;

          case ':':
            maybeToken = FlushBuffer(
              currentLexemeBuffer,
              ref absolutePosition,
              lineNumber,
              ref lexemeStartPositionInLine
            );
            if (maybeToken.IsSome)
              yield return maybeToken.ValueUnsafe();

            yield return source.Peek()
              .Filter(c => c == '=')
              .Some<IToken>(c =>
              {
                var result = new AssignmentOperatorToken(
                  absolutePosition,
                  lineNumber,
                  lexemeStartPositionInLine
                );

                source.Read();
                absolutePosition += 1;
                lexemeStartPositionInLine += 1;

                return result;
              })
              .None(new ColonSymbolToken(
                absolutePosition,
                lineNumber,
                lexemeStartPositionInLine
              ));

            absolutePosition += 1;
            lexemeStartPositionInLine += 1;

            break;

          case '!':
            maybeToken = FlushBuffer(
              currentLexemeBuffer,
              ref absolutePosition,
              lineNumber,
              ref lexemeStartPositionInLine
            );
            if (maybeToken.IsSome)
              yield return maybeToken.ValueUnsafe();

            yield return source.Peek()
              .Filter(c => c == '=')
              .Some<IToken>(_ =>
              {
                var result = new NotEqualsOperatorToken(
                  absolutePosition,
                  lineNumber,
                  lexemeStartPositionInLine
                );

                source.Read();
                absolutePosition += 1;
                lexemeStartPositionInLine += 1;

                return result;
              })
              .None(new UnrecognizedToken(
                "!",
                (uint) absolutePosition,
                lineNumber,
                lexemeStartPositionInLine
              ));

            absolutePosition += 1;
            lexemeStartPositionInLine += 1;

            break;

          case '>':
            maybeToken = FlushBuffer(
              currentLexemeBuffer,
              ref absolutePosition,
              lineNumber,
              ref lexemeStartPositionInLine
            );
            if (maybeToken.IsSome)
              yield return maybeToken.ValueUnsafe();

            yield return source.Peek()
              .Filter(c => c == '=')
              .Some<IToken>(_ =>
              {
                var result = new GeOperatorToken(
                  absolutePosition,
                  lineNumber,
                  lexemeStartPositionInLine
                );

                source.Read();
                absolutePosition += 1;
                lexemeStartPositionInLine += 1;

                return result;
              })
              .None(new GtOperatorToken(
                (uint) absolutePosition,
                lineNumber,
                lexemeStartPositionInLine
              ));

            absolutePosition += 1;
            lexemeStartPositionInLine += 1;

            break;

          case '<':
            maybeToken = FlushBuffer(
              currentLexemeBuffer,
              ref absolutePosition,
              lineNumber,
              ref lexemeStartPositionInLine
            );
            if (maybeToken.IsSome)
              yield return maybeToken.ValueUnsafe();

            yield return source.Peek()
              .Filter(c => c == '=')
              .Some<IToken>(_ =>
              {
                var result = new LeOperatorToken(
                  absolutePosition,
                  lineNumber,
                  lexemeStartPositionInLine
                );

                source.Read();
                absolutePosition += 1;
                lexemeStartPositionInLine += 1;

                return result;
              })
              .None(new LtOperatorToken(
                absolutePosition,
                lineNumber,
                lexemeStartPositionInLine
              ));

            absolutePosition += 1;
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
              ref absolutePosition,
              lineNumber,
              ref lexemeStartPositionInLine
            );
            if (maybeToken.IsSome)
              yield return maybeToken.ValueUnsafe();

            yield return SymbolLexemes
              .TryGetValue(((char) currentChar).ToString())
              .Some(cons => cons(
                absolutePosition,
                lineNumber,
                lexemeStartPositionInLine
              ))
              .None(() => new UnrecognizedToken(
                  currentChar.ToString(),
                  absolutePosition,
                  lineNumber,
                  lexemeStartPositionInLine
                )
              );

            absolutePosition += 1;
            lexemeStartPositionInLine += 1;

            break;

          default:
            currentLexemeBuffer.Append(char.ConvertFromUtf32(currentChar));
            break;
        }
      }

      maybeToken = FlushBuffer(
        currentLexemeBuffer,
        ref absolutePosition,
        lineNumber,
        ref lexemeStartPositionInLine
      );
      if (maybeToken.IsSome)
        yield return maybeToken.ValueUnsafe();
    }

    private Option<IToken> FlushBuffer(
      StringBuilder buffer,
      ref uint absolutePosition,
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

        absolutePosition += (uint) lexeme.Length;
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
