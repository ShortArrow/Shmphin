using main.model;
using Spectre.Console;

namespace main.ui;

class Ui
{
  public static Layout CreateLayout(string message)
  {
    var IsExMode = Input.Mode == InputMode.Ex;
    var IsNewValueMode = Input.Mode == InputMode.NewValue;
    // Create the layout
    var layout = new Layout("Root").SplitRows(
      new Layout("Header").Size(3),
      new Layout("Main").SplitColumns(
        new Layout("Left"),
        new Layout("Right").SplitRows(
          new Layout("Top"),
          new Layout("Bottom")
        )),
      new Layout("Footer").Size(3)
    );

    // Update the left column
    layout["Header"].Update(
      new Panel(Align.Center(
        new Markup($"[blue]{memory.Params.SharedMemoryName}[/]"),
        VerticalAlignment.Middle
      ))
      .Expand());
    layout["Main"]["Right"]["Top"].Update(
      new Panel(Align.Center(
        new Markup($"[green]{message}[/]"),
        VerticalAlignment.Middle
      ))
      .BorderColor(IsExMode ? Color.Default : Focus.TargetPanel == TargetPanel.Right ? Color.Green : Color.Default)
      .Expand()
    );
    layout["Main"]["Right"]["Bottom"].Update(
      new Panel(Align.Center(
        IsNewValueMode
          ? new Markup($"[red]{Input.inputBuffer}[/]")
          : new Markup($"[green]shmphin[/]"),
        VerticalAlignment.Middle
      ))
      .BorderColor(IsExMode ? Color.Default : Focus.TargetPanel == TargetPanel.Left ? Color.Green : Color.Default)
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
    var diff = new Matrix();
    diff.Update();

    // Create the grid
    var grid = new Grid();
    for (int i = 0; i < diff.Width; i++)
    {
      grid.AddColumn();
    }

    // Create the header
    var header = new List<Markup>();
    for (int i = 0; i < diff.Width; i++)
    {
      var color = i % 2 == 0 ? "blue" : "aqua";
      header.Add(new Markup($"[{color}]{i:X2}[/]"));
    }
    grid.AddRow([.. header]);

    // Create the main grid
    for (uint h = 0; h < diff.Height; h++)
    {
      var rowData = new List<Markup>();
      for (uint w = 0; w < diff.Width; w++)
      {
        var cell = diff.GetCell(w, h);
        string markup;
        if (cell.X == Cursor.X && cell.Y == Cursor.Y)
        {
          // Display foreground black and background white at the cursor position
          markup = $"[black on white]{cell.CurrentValue:X2}[/]";
        }
        else
        {
          // Display normal
          markup = (cell.BeforeValue == cell.CurrentValue)
            ? $"[white]{cell.CurrentValue:X2}[/]"
            : $"[red]{cell.CurrentValue:X2}[/]";
        }
        rowData.Add(new Markup(markup));
      }
      grid.AddRow([.. rowData]);
    }
    return grid;
  }
}