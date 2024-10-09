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
    var uts = new TaskCompletionSource<bool>(false);
    var cts = new CancellationTokenSource();
    var inputTask = Task.Run(() => ui.Input.InputLoop(cts, uts));
    var startMessage = new Markup("[bold green]Start Shmphin.[/]");
    await AnsiConsole.Live(startMessage)
    .StartAsync(async context =>
    {
      int counter = 0;
      while (!cts.Token.IsCancellationRequested)
      {
        var layout = ui.Ui.CreateLayout("normal");
        var updateTask = uts.Task;
        if (updateTask.IsCompleted)
        {
          layout = ui.Ui.CreateLayout("updated");
          memory.SnapShot.UpdateSnapShot();
          uts = new TaskCompletionSource<bool>(false);
        }

        context.UpdateTarget(layout);
        counter++;
        try
        {
          await Task.WhenAny(Task.Delay(1000, cts.Token), uts.Task);
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
