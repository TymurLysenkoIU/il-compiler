using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using FunctionalExtensions.IO;
using ILangCompiler.Parser.AST;
using ILangCompiler.Parser.AST.Declarations.Types.PrimitiveTypes;
using ILangCompiler.Parser.AST.Expressions;
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

    public CliApp(LexicalScanner lexicalScanner)
    {
      Lexer = lexicalScanner;
    }

    public async Task Main(string[] args) =>
      await (await CommandLine.Parser.Default.ParseArguments<CliOptions>(args)
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

        var tokens_copy = tokens.ToList(); 
        var parsers = ProgramNode.Parse(tokens_copy); 
        
        
        
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
          //         // $"Absolute position: {t.AbsolutePosition}", // Broken for now
          //         $"Line number: {t.LineNumber}",
          //         $"Position in line: {t.PositionInLine}",
          //       }
          //     )
          //   )
          // )
        ;

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

        resultEffect =
          FConsole.WriteLine(tokensString.ToString());
        
        
        
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