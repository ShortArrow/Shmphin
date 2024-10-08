using Spectre.Console;

namespace main;

class Program
{
  static Layout CreateLayout(String message)
  {
    // Create the layout
    var layout = new Layout("Root").SplitColumns(
        new Layout("Left"),
        new Layout("Right").SplitRows(new Layout("Top"), new Layout("Bottom")));

    // Update the left column
    layout["Left"].Update(
        new Panel(Align.Center(new Markup("Hello [blue]World![/]"),
                               VerticalAlignment.Middle))
            .Expand());
    layout["Right"]["Top"].Update(
        new Panel(Align.Center(new Markup($"[green]{message}[/]"),
                               VerticalAlignment.Middle))
            .Expand());
    return layout;
  }
  static async Task Main(string[] args)
  {
    string input = string.Empty;
    AnsiConsole.Clear();
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
