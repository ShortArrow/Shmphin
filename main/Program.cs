using System.CommandLine;

namespace main;

class Program
{
  static void Main(string[] args)
  {
    cli.Parser parser = new();
    parser.rootCommand.Invoke(args);
  }
}

