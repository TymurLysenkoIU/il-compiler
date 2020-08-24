This repository contains compiler for I-Lang (pronounced as Ы-lang 😃). This is an artificial language for practicing in compiler construction, which is a course at Innopolis University. The specification is available [here for registered users](https://moodle.innopolis.university/pluginfile.php/84643/mod_folder/content/0/Project%20I%20Eng.pdf?forcedownload=1).


# Project structure

## ILangCompiler

Compiler's source code.

### Dependency injection

The project uses a [dependency injection pattern](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/architectural-principles#dependency-inversion), so the `Program` class's `Main` function initializes the dependency injection framework and calls the `Main` function of `CliApp`class (thus, this is the actual program's entrypoint).

The simplest use case. To use a class as a dependency:
1. Create the desired class or at leas a stub for it
2. add it to the `IServiceCollection` in the `Program.RegisterServices` method
    - for example, see how it is done with `LexicalScanner`:
    ```cs
    serviceCollection.AddTransient<LexicalScanner>();
    ```
3. Add this class as a parameter to constructor of the class, which needs to use the API of the created class
    - e. g. `LexicalScanner` is used by `CliApp`, so its constructor looks like:
    ```cs
    public CliApp(LexicalScanner lexicalScanner ...
    ```
    The framework will automatically instantiate the `LexicalScanner`, when it will be creating `CliApp`, thus the term _dependency_ (`LexicalScanner` in this case) _injection_ (to the `CliApp` in this example).
4. You probably will want to use the injected class in the injectee's functions, so go store it in a property, which will be accessible by the class's functions
    - going back to example with `CliApp` and `LexicalScanner`:
    ```cs
    private LexicalScanner Lexer { get; }

    public CliApp(LexicalScanner lexicalScanner)
    {
      Lexer = lexicalScanner;
    }

    public void DoSomething()
    {
      // ...
      var result = LexicalScanner.Tokenize(input);
      // ...
    }
    ```

For more information on the DI, see the [docs for DI in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1) (Microsoft's web framework for .NET Core).

### Get command line arguments

To parse command line arguments the program uses [commandline](https://github.com/commandlineparser/commandline) library, so refer to [its documentation](https://github.com/commandlineparser/commandline/wiki) for details.

Class that contains options is called `CliOptions`. All command line options must reside there.
