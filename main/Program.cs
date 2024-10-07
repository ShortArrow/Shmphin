using Spectre.Console;

namespace main;

class Program
{
  static Layout CreateLayout()
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
    return layout;
  }
  static void Main(string[] args)
  {
    string input = string.Empty;
    AnsiConsole.Clear();
    var cts = new CancellationTokenSource();
    Task.Run(() =>
    {
      while (true)
      {
        var key = Console.ReadKey(true);
        if (key.Key.ToString().ToLower() == "q")
        {
          cts.Cancel();
          break;
        }
      }
    });
    var startMessage = new Markup("[bold green]Start Shmphin.[/]");
    AnsiConsole.Live(startMessage).Start(context =>
    {
      int counter = 0;
      while (!cts.Token.IsCancellationRequested)
      {
        context.UpdateTarget(CreateLayout());
        counter++;
        Thread.Sleep(1000);
      }
    });
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[bold red]Shmphin is Finished.[/]");
  }
}
