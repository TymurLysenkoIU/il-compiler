using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using FunctionalExtensions.IO;
using ILangCompiler.Scanner;
using LanguageExt;

// using ILangCompiler.Lexer;

namespace ILangCompiler
{
  public class CliApp
  {
    private LexicalScanner Lexer { get; }

    public CliApp(LexicalScanner lexicalScanner)
    {
      Lexer = lexicalScanner;
    }

    public async Task Main(string[] args) =>
      await (await Parser.Default.ParseArguments<CliOptions>(args)
        .WithParsedAsync(MainWithArgs))
        .WithNotParsedAsync(MainWithoutArgs)
      ;

    private async Task<Unit> MainWithArgs(CliOptions options)
    {
      Task<Unit> resultEffect = Task.FromResult(Unit.Default);

      // TODO: migrate effect to task
      if (File.Exists(options.FilePath))
      {
        // TODO: migrate effect to task
        using var fileReader = new SafeStreamReader(options.FilePath, Encoding.UTF8);
        var tokens = Lexer.Tokenize(fileReader);

        // TODO: add additional cli arguments to indicate the compilation result
        // TODO: add additional cli arguments to indicate the output file
        var tokensString = "[ " +
          string.Join(
            ", ",
            tokens.Select(t => $"({t.Lexeme}, {t.GetType().Name})")
          ) + " ]"
        ;

        resultEffect =
          FConsole.WriteLine(tokensString);
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