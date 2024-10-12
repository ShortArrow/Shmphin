using System.CommandLine;

namespace main;

class Program
{
  static Task<int> Main(string[] args)
  {
    cli.Parser parser = new();
    return parser.rootCommand.InvokeAsync(args);
  }
}

