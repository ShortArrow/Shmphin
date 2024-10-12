using System.CommandLine;
using System.CommandLine.Invocation;
using Spectre.Console;

namespace main.cli;

public class Handler
{
  internal static async Task TUI(string? sharedMemoryName, uint? sharedMemorySize, uint? sharedMemoryOffset, string? configFile)
  {
    AnsiConsole.Clear();
    if (sharedMemoryName == null)
    {
      ui.Welcome.Show();
      memory.Params.SharedMemoryName = ui.Input.GetSharedMemoryName();
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
    var inputTask = Task.Run(() => ui.Input.InputLoop());
    await AnsiConsole.Live(startMessage)
    .StartAsync(async context =>
    {
      while (!ui.Input.cts.Token.IsCancellationRequested)
      {
        var layout = ui.Ui.CreateLayout("normal");
        context.UpdateTarget(layout);
      }
      await inputTask;
    });
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[bold red]Shmphin is Finished.[/]");
  }

}