using Spectre.Console;
using main.config;
using System.CommandLine.Invocation;

namespace main.cli.handler;

public class Test : ICommandHandler
{
  private CurrentConfig? config;
  public Task<int> InvokeAsync(InvocationContext context)
  {
    var sharedMemoryName = context.ParseResult.GetValueForOption(Options.sharedMemoryName);
    var sharedMemorySize = context.ParseResult.GetValueForOption(Options.sharedMemorySize);
    var sharedMemoryOffset = context.ParseResult.GetValueForOption(Options.sharedMemoryOffset);
    var configFile = context.ParseResult.GetValueForOption(Options.configFile);
    var cellSize = context.ParseResult.GetValueForOption(Options.cellSize);
    var columnsLength = context.ParseResult.GetValueForOption(Options.columnsLength);
    config = new(new Command(sharedMemoryName, cellSize, columnsLength, sharedMemorySize, sharedMemoryOffset));
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[bold green]Start Shmphin Test.[/]");
    var grid = new Grid();

    // Add columns
    grid.AddColumn();
    grid.AddColumn();
    grid.AddColumn();

    // Add header row
    grid.AddRow(["Name", "Value"]);
    grid.AddRow([nameof(config.SharedMemoryName), config.SharedMemoryName ?? "null"]);
    grid.AddRow([nameof(config.SharedMemorySize), config.SharedMemorySize?.ToString() ?? "null"]);
    grid.AddRow([nameof(config.SharedMemoryOffset), config.SharedMemoryOffset?.ToString() ?? "null"]);
    grid.AddRow([nameof(configFile), configFile ?? "null"]);

    // Write to Console
    AnsiConsole.Write(grid);
    AnsiConsole.MarkupLine("[bold green]Shmphin is Finished.[/]");

    return Task.FromResult(0);
  }
  public int Invoke(InvocationContext context)
  {
    throw new NotSupportedException();
  }
}
