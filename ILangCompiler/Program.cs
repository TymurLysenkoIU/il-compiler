using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ILangCompiler.Scanner;

namespace ILangCompiler
{
  class Program
  {
    static Task Main(string[] args)
    {
      var serviceCollection = new ServiceCollection();
      serviceCollection.AddTransient<CliApp>();
      RegisterServices(serviceCollection);
      var serviceProvider = serviceCollection.BuildServiceProvider();

      var app = serviceProvider.GetService<CliApp>();
      return app.Main(args);
    }

    private static void RegisterServices(IServiceCollection serviceCollection)
    {
      // Register services for dependency injection here
      serviceCollection.AddTransient<LexicalScanner>();
    }
  }
}