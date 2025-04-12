using System.CommandLine;
using System.CommandLine.Parsing;

using main.cli;

namespace main;

public interface IApp
{
  Task<int> MainProcess(string[] args);
}

public class App(IParser parser, IConsole console) : IApp
{
  public async Task<int> MainProcess(string[] args)
  {
    try
    {
      return await parser.RootCommandWithSplash.InvokeAsync(args);
    }
    catch (Exception ex)
    {
      console.WriteLine(ex.Message);
      return 1;
    }
  }
}

