using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;

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

    private static void RegisterServices(ServiceCollection serviceCollection)
    {
      // Register services for dependency injection here
    }
  }
}