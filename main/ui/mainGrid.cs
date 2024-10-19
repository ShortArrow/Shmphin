using Spectre.Console;
using main.model;

namespace main.ui;
enum EvenOdd
{
  Odd,
  Even
}

class MainGrid
{
  private static EvenOdd currentRowColor = EvenOdd.Even;  // Start with zefo = Even
  private static void ToggleRowColor()
  {
    currentRowColor = currentRowColor == EvenOdd.Even ? EvenOdd.Odd : EvenOdd.Even;
  }
  private static Markup RowNumber(uint value, bool IsCurrentRow = false)
  {
    var high = value & 0xFFFF_0000;
    var low = value & 0x0000_FFFF;

    var color = (currentRowColor == EvenOdd.Even) ? "yellow" : "darkorange";
    var background = "default";
    if (IsCurrentRow)
    {
      background = color;
      color = "black";
    }
    return new($"[{color} on {background}]0x{high:X4}_{low:X4}[/]");
  }
  private static Markup HeaderNumber(uint value, EvenOdd evenodd)
  {
    var color = (evenodd == EvenOdd.Even) ? "blue" : "aqua";
    var background = "default";
    if (value == Cursor.X)
    {
      background = color;
      color = "black";
    }
    return new Markup($"[{color} on {background}]{value:X2}[/]");
  }
  public static Grid CreateDiffView()
  {
    var diff = new Matrix();
    diff.Update();

    // Create the grid
    var grid = new Grid
    {
      Expand = true
    };
    grid.AddColumn(); // Left row number column
    for (int i = 0; i < diff.Width; i++)
    {
      grid.AddColumn();
    }
    grid.AddColumn(); // Right row number column

    // Create the header
    var header = new List<Markup> { new("") }; // Empty cell for left row number column
    for (int i = 0; i < diff.Width; i++)
    {
      EvenOdd evenOdd = i % 2 == 0 ? EvenOdd.Even : EvenOdd.Odd;
      header.Add(HeaderNumber((uint)i, evenOdd));
    }
    header.Add(new Markup("")); // Empty cell for right row number column
    grid.AddRow([.. header]);

    // Create the main grid
    for (uint h = 0; h < diff.Height; h++)
    {
      var IsCurrentRow = Cursor.Y == h;

      var rowData = new List<Markup>
      { // Left row number
        RowNumber(h * diff.Width, IsCurrentRow)
      };

      for (uint w = 0; w < diff.Width; w++)
      {
        var cell = diff.GetCell(w, h);
        var formatted = cell.Length switch
        {
          1 => $"{cell.CurrentValue:X2}",
          2 => $"{cell.CurrentValue:X4}",
          4 => $"{cell.CurrentValue:X8}",
          8 => $"{cell.CurrentValue:X16}",
          _ => $"{cell.CurrentValue:X2}"
        };
        string markup;
        if (cell.X == Cursor.X && cell.Y == Cursor.Y)
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
      rowData.Add(RowNumber((h + 1) * diff.Width - 1, IsCurrentRow));
      grid.AddRow([.. rowData]);
      ToggleRowColor();
    }
    return grid;
  }
}
