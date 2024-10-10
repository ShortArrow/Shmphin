using System.Data;
using Spectre.Console;

namespace main.ui;

enum Focus
{
  header,
  main,
  footer
}

class Ui
{
  private static bool _isExMode = false;
  private static Focus _focus = Focus.main;
  public static bool IsExMode
  {
    get => _isExMode;
    set => _isExMode = value;
  }
  public static Focus Focus
  {
    get => _focus;
    set => _focus = value;
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
      .BorderColor(IsExMode ? Color.Red : Color.Default)
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
    var header = new List<Markup>();
    for (int i = 0; i < 8; i++)
    {
      var color = i % 2 == 0 ? "blue" : "aqua";
      header.Add(new Markup($"[{color}]{i:X2}[/]"));
    }
    grid.AddRow([.. header]);
    for (int i = 0; i < diff.Length; i += 8)
    {
      var rowData = new List<Markup>();
      for (int j = 0; j < 8 && (i + j) < diff.Length; j++)
      {
        var d = diff[i + j];
        var byteValue = d.Current;
        string markup;

        int currentX = j; // 列番号
        int currentY = i / 8; // 行番号

        if (currentX == Cursor.X && currentY == Cursor.Y)
        {
          // Cursorの位置の場合、前景黒、背景白で表示
          markup = $"[black on white]{byteValue:X2}[/]";
        }
        else
        {
          // 通常の表示
          markup = d.Before == d.Current
            ? $"[white]{byteValue:X2}[/]"
            : $"[red]{byteValue:X2}[/]";
        }

        rowData.Add(new Markup(markup));
      }

      grid.AddRow(rowData.ToArray());
    }

    return grid;
  }
}