using Spectre.Console;
using main.ui;
using main.operation;
using main.config;
using System.CommandLine.Invocation;

namespace main.cli.handler;

public class Test : ICommandHandler
{
  private CurrentConfig? config;
  private Operations? operations;
  private Input? input;
  private Ui? ui;
  private ui.Cursor? cursor;
  public Task<int> InvokeAsync(InvocationContext context)
  {
    var sharedMemoryName = context.ParseResult.GetValueForOption(Options.sharedMemoryName);
    var sharedMemorySize = context.ParseResult.GetValueForOption(Options.sharedMemorySize);
    var sharedMemoryOffset = context.ParseResult.GetValueForOption(Options.sharedMemoryOffset);
    var configFile = context.ParseResult.GetValueForOption(Options.configFile);
    var cellSize = context.ParseResult.GetValueForOption(Options.cellSize);
    var columnsLength = context.ParseResult.GetValueForOption(Options.columnsLength);
    config = new(new Command(sharedMemoryName, cellSize, columnsLength, sharedMemorySize, sharedMemoryOffset));
    cursor = new(config);
    operations = new(config, cursor);
    input = new(config, cursor);
    ui = new(config, cursor);
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[bold green]Start Shmphin Test.[/]");
    var grid = new Grid();

    // Add columns
    grid.AddColumn();
    grid.AddColumn();
    grid.AddColumn();

    // Add header row
    grid.AddRow(["Name", "Value"]);
    grid.AddRow([nameof(sharedMemoryName), sharedMemoryName ?? "null"]);
    grid.AddRow([nameof(sharedMemorySize), sharedMemorySize?.ToString() ?? "null"]);
    grid.AddRow([nameof(sharedMemoryOffset), sharedMemoryOffset?.ToString() ?? "null"]);
    grid.AddRow([nameof(configFile), configFile ?? "null"]);
    grid.AddRow([nameof(input.InputBuffer), input.InputBuffer ?? "null"]);
    grid.AddRow([nameof(operations.UpdateMemory), operations.UpdateMemory.ToString() ?? "null"]);
    grid.AddRow([nameof(ui), ui.ToString() ?? "null"]);

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
