using Spectre.Console;
using main.ui;
using main.operation;

namespace main.cli;

public class Handler
{
  readonly config.CurrentConfig config = new();
  internal async Task TUI(string? sharedMemoryName, uint? sharedMemorySize, uint? sharedMemoryOffset, string? configFile)
  {
    AnsiConsole.Clear();
    if (sharedMemoryName == null)
    {
      Welcome.Show();
      memory.Params.SharedMemoryName = Input.GetSharedMemoryName();
    }
    if (sharedMemorySize != null)
    {
      memory.Params.Size = (uint)sharedMemorySize;
    }
    if (sharedMemoryOffset != null)
    {
      memory.Params.Offset = (uint)sharedMemoryOffset;
    }
    if (configFile != null)
    {
      config.UpdateConfig(configFile);
    }
    var startMessage = new Markup("[bold green]Start Shmphin.[/]");
    var inputTask = Task.Run(() => Input.InputLoop());
    await AnsiConsole.Live(startMessage)
    .StartAsync(async context =>
    {
      Operations.UpdateMemory.Execute();
      while (!Input.IsCancellationRequested)
      {
        var layout = Ui.CreateLayout("normal");
        context.UpdateTarget(layout);
      }
      await inputTask;
    });
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[bold red]Shmphin is Finished.[/]");
  }

}
