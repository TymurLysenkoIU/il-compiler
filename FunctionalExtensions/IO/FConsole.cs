using System;
using System.Threading.Tasks;
using LanguageExt;

namespace FunctionalExtensions.IO
{
  public static class FConsole
  {
    public static async Task<Unit> WriteLine(string value)
    {
      Console.WriteLine(value);
      return Unit.Default;
    }
  }
}