using Spectre.Console;
using main.ui;
using main.operation;
using main.config;
using System.CommandLine.Invocation;
using main.memory;

namespace main.cli.handler;

public class Root : ICommandHandler
{
  private CurrentConfig? config;
  private Operations? operations;
  private Input? input;
  private Ui? ui;
  private model.Cursor? cursor;
  private SnapShot? snapShot;
  private Memory? memory;
  private Mode? mode;
  private Question? question;
  private Focus? focus;
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
    config = new(new Command(sharedMemoryName, cellSize, columnsLength, sharedMemorySize, sharedMemoryOffset));
    question = new(config);
    cursor = new(config);
    memory = new(config);
    mode = new();
    focus = new();
    snapShot = new(config);
    operations = new(config, cursor, memory, snapShot, mode, focus);
    input = new(operations, mode);
    ui = new(config, cursor, snapShot, focus, mode);
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
