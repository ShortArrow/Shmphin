using System.Runtime.Versioning;
using Spectre.Console;

namespace main;

[SupportedOSPlatform("windows")]
class Program
{
  private static byte[] bytes = new byte[8];
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
        var updateTask = ui.Input.uts.Task;
        if (updateTask.IsCompleted)
        {
          layout = ui.Ui.CreateLayout("updated");
          memory.SnapShot.UpdateSnapShot();
          ui.Input.uts = new TaskCompletionSource<bool>(false);
        }

        context.UpdateTarget(layout);
        counter++;
        try
        {
          await Task.WhenAny(Task.Delay(100, ui.Input.cts.Token), ui.Input.uts.Task);
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
