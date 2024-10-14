using Spectre.Console;
using main.ui;

namespace main.cli;

public class Handler
{
  internal static async Task TUI(string? sharedMemoryName, uint? sharedMemorySize, uint? sharedMemoryOffset, string? configFile)
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
      config.Config.UpdateConfig(configFile);
    }
    var startMessage = new Markup("[bold green]Start Shmphin.[/]");
    var inputTask = Task.Run(() => Input.InputLoop());
    await AnsiConsole.Live(startMessage)
    .StartAsync(async context =>
    {
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