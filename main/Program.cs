using System.Runtime.Versioning;
using Microsoft.VisualBasic;
using Spectre.Console;

namespace main;

[SupportedOSPlatform("windows")]
class Program
{
  private static string sharedMemoryName = string.Empty;
  private static byte[] bytes= new byte[8];
  static Layout CreateLayout(string message)
  {
    // Create the layout
    var layout = new Layout("Root").SplitColumns(
        new Layout("Left"),
        new Layout("Right").SplitRows(new Layout("Top"), new Layout("Bottom")));

    // Update the left column
    layout["Left"].Update(
        new Panel(Align.Center(new Markup($"[blue]{sharedMemoryName}[/]"),
                               VerticalAlignment.Middle))
            .Expand());
    layout["Right"]["Top"].Update(
        new Panel(Align.Center(new Markup($"[green]{message}[/]"),
                               VerticalAlignment.Middle))
            .Expand());
    layout["Right"]["Bottom"].Update(
        new Panel(Align.Center(new Markup($"[yellow]{BitConverter.ToString(bytes)}[/]"),
                               VerticalAlignment.Middle))
            .Expand());
    return layout;
  }
  static async Task Main(string[] args)
  {
    var input = string.Empty;
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine($"Welcome! [bold green]Shmphin[/] is a shared memory editor.");
    sharedMemoryName = AnsiConsole.Prompt(
      new TextPrompt<string>("Enter the shared memory name:")
    );
    var uts = new TaskCompletionSource<bool>(false);
    var cts = new CancellationTokenSource();
    var inputTask = Task.Run(() =>
    {
      while (!cts.Token.IsCancellationRequested)
      {
        var key = Console.ReadKey(true);
        if (key.KeyChar.ToString()
        .Equals("q", StringComparison.CurrentCultureIgnoreCase))
        {
          cts.Cancel(); // Request cancellation
        }
        else if (key.KeyChar.ToString().Equals("u", StringComparison.CurrentCultureIgnoreCase))
        {
          uts.TrySetResult(true);
        }
      }
    });
    var startMessage = new Markup("[bold green]Start Shmphin.[/]");
    await AnsiConsole.Live(startMessage)
    .StartAsync(async context =>
    {
      int counter = 0;
      while (!cts.Token.IsCancellationRequested)
      {
        var layout = CreateLayout("normal");
        var updateTask = uts.Task;
        if (updateTask.IsCompleted)
        {
          layout = CreateLayout("updated");
          var buffer = memory.SharedMemoryHelper.ReadFromSharedMemory(sharedMemoryName, 0, 8);
          bytes = buffer;
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
