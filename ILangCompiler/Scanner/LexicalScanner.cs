using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ILangCompiler.Scanner.Tokens;

namespace ILangCompiler.Scanner
{
  public class LexicalScanner
  {
    public async IAsyncEnumerable<CharToken> Tokenize(StreamReader source)
    {
      // TODO: process source and return list of tokens
      while (!source.EndOfStream)
      {
        yield return new CharToken((char) source.Read());
      }
    }
  }
}