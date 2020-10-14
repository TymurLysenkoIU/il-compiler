using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using FunctionalExtensions.IO;
using ILangCompiler.Parser.AST;
using ILangCompiler.Scanner;
using ILangCompiler.Scanner.Tokens;
using ILangCompiler.Scanner.Tokens.Literals;
using ILangCompiler.Scanner.Tokens.Predefined.Symbols;
using LanguageExt;

namespace ILangCompiler
{
  public class CliApp
  {
    private LexicalScanner Lexer { get; }
    private SemanticAnalyzer.SemanticAnalyzer SemAnalyzer { get; }

    public CliApp(LexicalScanner lexicalScanner, SemanticAnalyzer.SemanticAnalyzer semanticAnalyzer)
    {
      Lexer = lexicalScanner;
      SemAnalyzer = semanticAnalyzer;
    }

    public async Task Main(string[] args) =>
      await (await CommandLine.Parser.Default.ParseArguments<CliOptions>(args)
        .WithParsedAsync(MainWithArgs))
        .WithNotParsedAsync(MainWithoutArgs)
      ;

    private async Task<Unit> MainWithArgs(CliOptions options)
    {
      Task<Unit> resultEffect = Task.FromResult(Unit.Default);

      if (File.Exists(options.FilePath))
      {
        using var fileReader = new SafeStreamReader(options.FilePath, Encoding.UTF8);
        var tokens = Lexer.Tokenize(fileReader).ToList();


        // TODO: add additional cli arguments to indicate the compilation result
        // TODO: add additional cli arguments to indicate the output file
        var tokensString = new StringBuilder();

        // string.Join(
        //   "\n",
        //   tokens.Select(t =>
        //     string.Join(", ",
        //       new []
        //       {
        //         "Lexeme: " + (t.Lexeme == "\n" ? @"\n" : t.Lexeme),
        //         $"Token: {t.GetType().Name}",
        //         // $"Absolute position: {t.AbsolutePosition}",
        //         $"Line number: {t.LineNumber}",
        //         $"Position in line: {t.PositionInLine}",
        //       }
        //     )
        //   )
        // );

        foreach (var token in tokens)
        {
          switch (token)
          {
            case NewLineSymbolToken t:
              tokensString.Append("\n");
              break;

            case IdentifierToken t:
            case LiteralToken _:
              tokensString.Append($"{{{token.GetType().Name} [{token.Lexeme}] ({token.LineNumber}, {token.PositionInLine}, {token.AbsolutePosition})}} ");
              break;

            default:
              tokensString.Append($"{{{token.GetType().Name} ({token.LineNumber}, {token.PositionInLine}, {token.AbsolutePosition})}} ");
              break;
          }
        }

        var ast = ProgramNode.Parse(tokens);

        resultEffect = ast
          .MapLeft(
            e => FConsole.WriteLine($"An error occurred during parsing: {e.Message}")
          )
          .Map(SemAnalyzer.Analyze)
          .Map(errors => errors
            .Fold(
              FConsole.WriteLine(
                errors.Count > 0 ?
                  "Semantic errors:" :
                  "No semantic errors occured!"
              ),
              (_, e) => FConsole.WriteLine($"{e.Message}")
            )
          )
          .Match(
            Left: e => e,
            Right: e => e
          );
      }
      else
      {
        resultEffect =
          FConsole.WriteLine($"Error: there is no file {options.FilePath}");
      }

      return await resultEffect;
    }

    private Task MainWithoutArgs(IEnumerable<Error> errors) =>
      Task.CompletedTask;
  }
}