using Spectre.Console;

namespace main;

class Program
{
  static Layout CreateLayout()
  {
    // Create the layout
    var layout = new Layout("Root")
        .SplitColumns(
            new Layout("Left"),
            new Layout("Right")
                .SplitRows(
                    new Layout("Top"),
                    new Layout("Bottom")));

    // Update the left column
    layout["Left"].Update(
        new Panel(
            Align.Center(
                new Markup("Hello [blue]World![/]"),
                VerticalAlignment.Middle))
            .Expand());
    return layout;
  }
  static void Main(string[] args)
  {
    string input = string.Empty;
    AnsiConsole.Clear();
    AnsiConsole.Live(new Markup("[bold green]Start Shmphin.[/]"))
    .Start(ctx =>
    {
      int counter = 0;
      var inputTask = new Thread(() =>
      {
        while (input != "q")
        {
          var key = Console.ReadKey(true);
          input = key.KeyChar.ToString().ToLower();

        }
      });
      inputTask.Start();
      while (input != "q")
      {
        ctx.UpdateTarget(CreateLayout());
        Thread.Sleep(1000);
        counter++;
      }
      inputTask.Join();
    });
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[bold red]Shmphin is Finished.[/]");
  }
}
