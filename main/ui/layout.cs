using System.Data;
using Spectre.Console;

namespace main.ui;

class Ui
{
  public static Layout CreateLayout(string message)
  {
    // Create the layout
    var layout = new Layout("Root").SplitRows(
      new Layout("Header").Size(3),
      new Layout("Main").SplitColumns(
        new Layout("Left"),
        new Layout("Right")),
      new Layout("Footer").Size(3)
    );

    // Update the left column
    layout["Header"].Update(
        new Panel(Align.Center(new Markup($"[blue]{memory.Params.SharedMemoryName}[/]"),
                               VerticalAlignment.Middle))
            .Expand());
    layout["Main"]["Right"].Update(
        new Panel(Align.Center(new Markup($"[green]{message}[/]"),
                               VerticalAlignment.Middle))
            .Expand());
    layout["Main"]["Left"].Update(
        new Panel(Align.Center(new Markup($"[yellow]{BitConverter.ToString(memory.SnapShot.Current)}[/]"),
                               VerticalAlignment.Middle))
            .Expand());
    layout["Footer"].Update(
        new Panel(Align.Center(new Markup($"[green]Shmphin[/]"),
                               VerticalAlignment.Middle))
            .Expand());
    return layout;
  }
  private void CreateDiffView()
  {

  }
}