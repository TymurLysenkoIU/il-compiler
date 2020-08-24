using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace ILangCompiler
{
  public class CliApp
  {
    // TODO: inject lexer and use it

    public async Task Main(string[] args) =>
      await (await Parser.Default.ParseArguments<CliOptions>(args)
        .WithParsedAsync(MainWithArgs))
        .WithNotParsedAsync(MainWithoutArgs)
      ;

    private Task MainWithArgs(CliOptions options)
    {
      if (File.Exists(options.FilePath))
      {
        using var fileReader = new StreamReader(options.FilePath, Encoding.UTF8);
        // TODO: use lexer
      }
      else
      {
        return Task.Run(() =>
          Console.WriteLine($"Error: there is no file {options.FilePath}")
        );
      }

      return Task.CompletedTask;
    }

    private Task MainWithoutArgs(IEnumerable<Error> errors) =>
      Task.CompletedTask;
  }
}