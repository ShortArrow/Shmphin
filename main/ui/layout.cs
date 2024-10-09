using System.Data;
using Spectre.Console;

namespace main.ui;

class Ui
{
  public static Layout CreateLayout(string message)
  {
    // Create the layout
    var layout = new Layout("Root").SplitColumns(
        new Layout("Left"),
        new Layout("Right").SplitRows(new Layout("Top"), new Layout("Bottom")));

    // Update the left column
    layout["Left"].Update(
        new Panel(Align.Center(new Markup($"[blue]{memory.Params.SharedMemoryName}[/]"),
                               VerticalAlignment.Middle))
            .Expand());
    layout["Right"]["Top"].Update(
        new Panel(Align.Center(new Markup($"[green]{message}[/]"),
                               VerticalAlignment.Middle))
            .Expand());
    layout["Right"]["Bottom"].Update(
        new Panel(Align.Center(new Markup($"[yellow]{BitConverter.ToString(memory.SnapShot.Current)}[/]"),
                               VerticalAlignment.Middle))
            .Expand());
    return layout;
  }
  private void CreateDiffView()
  {

  }
}