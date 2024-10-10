using System.Data;
using Spectre.Console;

namespace main.ui;

class Ui
{
  private static bool _isExMode = false;
  public static bool IsExMode
  {
    get => _isExMode;
    set => _isExMode = value;
  }
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
      new Panel(Align.Center(
        new Markup($"[blue]{memory.Params.SharedMemoryName}[/]"),
        VerticalAlignment.Middle
      ))
      .Expand());
    layout["Main"]["Right"].Update(
      new Panel(Align.Center(
        new Markup($"[green]{message}[/]"),
        VerticalAlignment.Middle
      ))
      .Expand()
    );
    layout["Main"]["Left"].Update(
      new Panel(Align.Center(
        CreateDiffView(),
        VerticalAlignment.Middle
      ))
      .BorderColor(IsExMode ? Color.Default : Color.Green)
      .Expand()
    );
    layout["Footer"].Update(
      new Panel(Align.Center(
        new Markup($"[green]{Input.inputBuffer}[/]"),
        VerticalAlignment.Middle
      ))
      .BorderColor(IsExMode ? Color.Green : Color.Default)
      .Expand()
    );
    return layout;
  }
  private static Grid CreateDiffView()
  {
    var before = memory.SnapShot.Before;
    var current = memory.SnapShot.Current;
    var diff = before.Zip(current, (b, c) => (Before: b, Current: c)).ToArray();
    var grid = new Grid();
    for (int i = 0; i < 8; i++)
    {
      grid.AddColumn();
    }
    for (int i = 0; i < diff.Length; i += 8)
    {
      var row = diff.Skip(i).Take(8).Select(d =>
      {
        var byteValue = d.Current;
        var markup = d.Before == d.Current
          ? $"[yellow]{byteValue:X2}[/]"
          : $"[red]{byteValue:X2}[/]";
        return new Markup(markup);
      }).ToArray();
      grid.AddRow(row);
    }
    return grid;
  }
}