using Spectre.Console;

namespace main.ui;

class Ui
{
  private static bool isExMode = false;
  public static bool IsExMode
  {
    get => isExMode;
    set => isExMode = value;
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
      .BorderColor(IsExMode ? Color.Default : Focus.TargetPanel == TargetPanel.Right ? Color.Green : Color.Default)
      .Expand()
    );
    layout["Main"]["Left"].Update(
      new Panel(Align.Center(
        CreateDiffView(),
        VerticalAlignment.Middle
      ))
      .BorderColor(IsExMode ? Color.Default : Focus.TargetPanel == TargetPanel.Left ? Color.Green : Color.Default)
      .Expand()
    );
    layout["Footer"].Update(
      new Panel(Align.Center(
        new Markup($"[green]{Input.inputBuffer}[/]"),
        VerticalAlignment.Middle
      ))
      .BorderColor(IsExMode ? Color.Red : Color.Default)
      .Expand()
    );
    return layout;
  }
  private static Grid CreateDiffView()
  {
    // Create set of memory snapshots
    var before = memory.SnapShot.Before;
    var current = memory.SnapShot.Current;
    var diff = before.Zip(current, (b, c) => (Before: b, Current: c)).ToArray();

    // Create the grid
    var grid = new Grid();
    var columnsQuantity = GridMode.ColumnsLength;
    for (int i = 0; i < columnsQuantity; i++)
    {
      grid.AddColumn();
    }

    // Create the header
    var header = new List<Markup>();
    for (int i = 0; i < columnsQuantity; i++)
    {
      var color = i % 2 == 0 ? "blue" : "aqua";
      header.Add(new Markup($"[{color}]{i:X2}[/]"));
    }
    grid.AddRow([.. header]);

    // Create the main grid
    for (uint i = 0; i < diff.Length; i += columnsQuantity)
    {
      var rowData = new List<Markup>();
      for (uint j = 0; j < columnsQuantity && (i + j) < diff.Length; j++)
      {
        var (Before, Current) = diff[i + j];
        string markup;
        uint currentX = j; // Column number
        uint currentY = i / columnsQuantity; // Row number
        if (currentX == Cursor.X && currentY == Cursor.Y)
        {
          // Display foreground black and background white at the cursor position
          markup = $"[black on white]{Current:X2}[/]";
        }
        else
        {
          // Display normal
          markup = Before == Current
            ? $"[white]{Current:X2}[/]"
            : $"[red]{Current:X2}[/]";
        }
        rowData.Add(new Markup(markup));
      }
      grid.AddRow([.. rowData]);
    }
    return grid;
  }
}