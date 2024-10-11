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
    var inputTask = Task.Run(() => ui.Input.InputLoop());
    var startMessage = new Markup("[bold green]Start Shmphin.[/]");
    await AnsiConsole.Live(startMessage)
    .StartAsync(async context =>
    {
      int counter = 0;
      while (!ui.Input.cts.Token.IsCancellationRequested)
      {
        var layout = ui.Ui.CreateLayout("normal");

        context.UpdateTarget(layout);
        counter++;
        try
        {
          await Task.Delay(1, ui.Input.cts.Token);
        }
        catch (TaskCanceledException)
        {
          // No thing to do
        }
      }
      await inputTask;
    });
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[bold red]Shmphin is Finished.[/]");
  }
}
