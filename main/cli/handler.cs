using Spectre.Console;
using main.ui;
using main.operation;
using main.config;

namespace main.cli;

public class Handler
{
  private CurrentConfig? config;
  private Operations? operations;
  private Input? input;
  private Ui? ui;
  private ui.Cursor? cursor;
  internal async Task TUI(string? sharedMemoryName, uint? sharedMemorySize, uint? sharedMemoryOffset, string? configFile)
  {
    config = new(new Command(sharedMemoryName, 8, 1, sharedMemorySize, sharedMemoryOffset)); // TODO: use cli flags
    cursor = new(config);
    operations = new(config, cursor);
    input = new(config);
    ui = new(config, cursor);
    AnsiConsole.Clear();
    if (config.SharedMemoryName == null)
    {
      Welcome.Show();
      config.SharedMemoryName = Input.GetSharedMemoryName();
    }
    config.UpdateConfig(configFile);
    var startMessage = new Markup("[bold green]Start Shmphin.[/]");
    var inputTask = Task.Run(() => input.InputLoop());
    await AnsiConsole.Live(startMessage)
    .StartAsync(async context =>
    {
      operations.UpdateMemory.Execute();
      while (!input.IsCancellationRequested)
      {
        var layout = ui.CreateLayout(config, input);
        context.UpdateTarget(layout);
      }
      await inputTask;
    });
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[bold red]Shmphin is Finished.[/]");
  }

}
