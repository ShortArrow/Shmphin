using Spectre.Console;

namespace main;

class Program
{
  static void Main(string[] args)
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

    AnsiConsole.Write(layout);
  }
}
