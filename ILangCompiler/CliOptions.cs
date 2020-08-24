using CommandLine;

namespace ILangCompiler
{
  public class CliOptions
  {
    private readonly string filePath;

    public CliOptions(string filePath)
    {
      this.filePath = filePath;
    }

    [Value(0,
      Required = true,
      HelpText = "Path to the file with the source code to compile.",
      Hidden = false,
      MetaName = "file-path",
      MetaValue = "FILE-PATH"
    )]
    public string FilePath => filePath;
  }
}