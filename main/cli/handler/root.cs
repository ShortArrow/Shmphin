using Spectre.Console;
using main.ui;
using main.config;
using System.CommandLine.Invocation;
using main.ui.keyhandler;
using main.ui.layout;
using main.operation;

namespace main.cli.handler;

public interface IRoot : ICommandHandler
{
}

public class Root(ICurrentConfig config, IQuestion question, IInput input, IMode mode, IUi ui, IOperations operations) : IRoot
{
  public int Invoke(InvocationContext context)
  {
    throw new NotSupportedException();
  }
  public async Task<int> InvokeAsync(InvocationContext context)
  {
    var sharedMemoryName = context.ParseResult.GetValueForOption(Options.sharedMemoryName);
    var sharedMemorySize = context.ParseResult.GetValueForOption(Options.sharedMemorySize);
    var sharedMemoryOffset = context.ParseResult.GetValueForOption(Options.sharedMemoryOffset);
    var configFile = context.ParseResult.GetValueForOption(Options.configFile);
    var cellSize = context.ParseResult.GetValueForOption(Options.cellSize);
    var columnsLength = context.ParseResult.GetValueForOption(Options.columnsLength);
    config = new CurrentConfig(new Args(sharedMemoryName, cellSize, columnsLength, sharedMemorySize, sharedMemoryOffset));
    AnsiConsole.Clear();
    if (config.SharedMemoryName == null)
    {
      Welcome.Show();
      await question.GetSharedMemoryName();
    }
    config.UpdateConfig(configFile);
    var startMessage = new Markup("[bold green]Start Shmphin.[/]");
    var inputTask = Task.Run(() => input.InputLoop());
    await AnsiConsole.Live(startMessage)
    .StartAsync(async context =>
    {
      operations.UpdateMemory.Execute();
      while (!mode.IsCancellationRequested)
      {
        var layout = ui.CreateLayout(config, input);
        context.UpdateTarget(layout);
      }
      await inputTask;
    });
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[bold red]Shmphin is Finished.[/]");
    return 0;
  }
}
