using main.model;
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
    var diff = new Matrix();
    diff.Update();
    for (uint i = 0; i < diff.Width; i++)
    {
      var rowData = new List<Markup>();
      for (uint j = 0; j < diff.Height; j++)
      {
        var cell = diff.GetCell(j, i);
        string markup;
        if (cell.X == Cursor.X && cell.Y == Cursor.Y)
        {
          // Display foreground black and background white at the cursor position
          markup = $"[black on white]{cell.Current:X2}[/]";
        }
        else
        {
          // Display normal
          markup = cell.Before == cell.Current
            ? $"[white]{cell.Current:X2}[/]"
            : $"[red]{cell.Current:X2}[/]";
        }
        rowData.Add(new Markup(markup));
      }
      grid.AddRow([.. rowData]);
    }
    return grid;
  }
}