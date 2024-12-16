using Spectre.Console;
using main.model;
using main.config;
using main.memory;

namespace main.ui;
enum EvenOdd
{
  Odd,
  Even
}

class MainGrid
{
  public MainGrid(IConfig config, Cursor cursor, SnapShot snapShot, Focus focus)
  {
    matrix = new(config, snapShot);
    cursorInfo = new(cursor, matrix, focus, FormatAddress);
    this.cursor = cursor;
  }
  private readonly Cursor cursor;
  private readonly Matrix matrix;
  public Matrix Matrix => matrix;
  private EvenOdd currentRowColor = EvenOdd.Even;  // Start with zefo = Even
  private void ToggleRowColor()
  {
    currentRowColor = currentRowColor == EvenOdd.Even ? EvenOdd.Odd : EvenOdd.Even;
  }
  private readonly CursorInfo cursorInfo;
  public Grid CursorInfoView => cursorInfo.CreateCursorView();
  private static string FormatAddress(uint value)
  {
    var high = value & 0xFFFF_0000;
    var low = value & 0x0000_FFFF;
    return $"0x{high:X4}_{low:X4}";
  }
  private Markup RowNumber(uint value, bool IsCurrentRow = false)
  {
    var address = FormatAddress(value);
    var color = (currentRowColor == EvenOdd.Even) ? "yellow" : "darkorange";
    var foreground = IsCurrentRow ? "black" : color;
    var background = IsCurrentRow ? color : "default";
    return new($"[{foreground} on {background}]{address}[/]");
  }
  private Markup HeaderNumber(uint value, EvenOdd evenodd)
  {
    var color = (evenodd == EvenOdd.Even) ? "blue" : "aqua";
    var IsCurrentColumn = cursor.X == value;
    var foreground = IsCurrentColumn ? "black" : color;
    var background = IsCurrentColumn ? color : "default";
    return new Markup($"[{foreground} on {background}]{value:X2}[/]");
  }
  public Grid CreateDiffView()
  {
    matrix.Update();

    // Create the grid
    var grid = new Grid
    {
      Expand = true
    };
    grid.AddColumn(); // Left row number column
    for (int i = 0; i < matrix.Width; i++)
    {
      grid.AddColumn();
    }
    grid.AddColumn(); // Right row number column

    // Create the header
    var header = new List<Markup> { new("") }; // Empty cell for left row number column
    for (int i = 0; i < matrix.Width; i++)
    {
      EvenOdd evenOdd = i % 2 == 0 ? EvenOdd.Even : EvenOdd.Odd;
      header.Add(HeaderNumber((uint)i, evenOdd));
    }
    header.Add(new Markup("")); // Empty cell for right row number column
    grid.AddRow([.. header]);

    // Create the main grid
    for (uint h = 0; h < matrix.Height; h++)
    {
      var IsCurrentRow = cursor.Y == h;

      var rowData = new List<Markup>
      { // Left row number
        RowNumber(h * matrix.Width, IsCurrentRow)
      };

      for (uint w = 0; w < matrix.Width; w++)
      {
        var cell = matrix.GetCell(w, h);
        var formatted = cell.Length switch
        {
          1 => $"{cell.CurrentValue:X2}",
          2 => $"{cell.CurrentValue:X4}",
          4 => $"{cell.CurrentValue:X8}",
          8 => $"{cell.CurrentValue:X16}",
          _ => $"{cell.CurrentValue:X2}"
        };
        string markup;
        if (cell.X == cursor.X && cell.Y == cursor.Y)
        { // Display cursor position
          markup = $"[black on white]{formatted}[/]";
        }
        else
        { // Display normal
          markup = (cell.BeforeValue == cell.CurrentValue)
              ? $"[white]{formatted}[/]"
              : $"[red]{formatted}[/]";
        }
        rowData.Add(new Markup(markup));
      }

      // Right row number
      rowData.Add(RowNumber((h + 1) * matrix.Width - 1, IsCurrentRow));
      grid.AddRow([.. rowData]);
      ToggleRowColor();
    }
    return grid;
  }
}
