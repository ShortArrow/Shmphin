using System.CommandLine;
using System.CommandLine.Parsing;

namespace main;

public interface IApp
{
  Task<int> MainProcess(string[] args);
}

public class App : IApp
{
  public async Task<int> MainProcess(string[] args)
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

