using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Parsing;

namespace main;

class Program
{
  static async Task<int> Main(string[] args)
  {
    try
    {
      var parser = new cli.Parser();
      return await parser.rootCommandWithSplash.InvokeAsync(args);
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
      return 1;
    }
  }
}

