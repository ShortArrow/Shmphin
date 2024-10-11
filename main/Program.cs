using System.Runtime.Versioning;
using Spectre.Console;

namespace main;

[SupportedOSPlatform("windows")]
class Program
{
  static async Task Main(string[] args)
  {
    var input = string.Empty;
    AnsiConsole.Clear();
    ui.Welcome.Show();
    memory.Params.SharedMemoryName = ui.Input.GetSharedMemoryName();
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
