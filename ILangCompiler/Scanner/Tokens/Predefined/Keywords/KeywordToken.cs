using System;
using System.Collections.Generic;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.Constructions;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.Declaration;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.LogicalOperations;
using ILangCompiler.Scanner.Tokens.Predefined.Keywords.PrimitiveTypes;
using LanguageExt;

namespace ILangCompiler.Scanner.Tokens.Predefined.Keywords
{
  public abstract class KeywordToken : PredefinedToken
  {
    public KeywordToken(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {
    }

    public static Option<IToken> FromString(
      string s,
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) =>
      KeywordLexemes.TryGetValue(s)
        .Map(cons =>
          cons(
            absolutePosition,
            lineNumber,
            positionInLine
          )
        );

    public static IReadOnlyDictionary<
      string,
      Func<uint, uint, uint, IToken>
    > KeywordLexemes { get; } =
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
        [ReturnKeywordToken.LexemeValue] =
          (ap, ln, pl) =>
            new ReturnKeywordToken(
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
      };
  }
}