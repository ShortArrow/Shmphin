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
  private static readonly Matrix diff = new();
  private static EvenOdd currentRowColor = EvenOdd.Even;  // Start with zefo = Even
  private static void ToggleRowColor()
  {
    currentRowColor = currentRowColor == EvenOdd.Even ? EvenOdd.Odd : EvenOdd.Even;
  }
  public static Grid CreateCursorView()
  {
    diff.Update();
    var grid = new Grid();
    var index = Matrix.GetIndex(Cursor.X, Cursor.Y);

    grid.AddColumns(2);
    grid.AddRow(new Markup($"[green bold]Name[/]"), new Markup($"[red bold]Value[/]"));
    var address = FromatAddress((uint)index);
    var dict = new Dictionary<string, string>{
      {"x", $"{Cursor.X}"},
      {"y", $"{Cursor.Y}"},
      {"byteIndex", $"{index}"},
      {"wordIndex", $"{index / 2}"},
      {"BeforeValue", $"{diff.GetCell(Cursor.X, Cursor.Y).BeforeValue}"},
      {"CurrentValue", $"{diff.GetCell(Cursor.X, Cursor.Y).CurrentValue}"},
      {"Address", $"{address}"}
    };
    foreach (var item in dict)
    {
      grid.AddRow(new Text(item.Key), new Text(item.Value).RightJustified());
    }
    return grid;
  }
  private static string FromatAddress(uint value)
  {
    var high = value & 0xFFFF_0000;
    var low = value & 0x0000_FFFF;
    return $"0x{high:X4}_{low:X4}";
  }
  private static Markup RowNumber(uint value, bool IsCurrentRow = false)
  {
    var address = FromatAddress(value);
    var color = (currentRowColor == EvenOdd.Even) ? "yellow" : "darkorange";
    var foreground = IsCurrentRow ? "black" : color;
    var background = IsCurrentRow ? color : "default";
    return new($"[{foreground} on {background}]{address}[/]");
  }
  private static Markup HeaderNumber(uint value, EvenOdd evenodd)
  {
    var color = (evenodd == EvenOdd.Even) ? "blue" : "aqua";
    var IsCurrentColumn = Cursor.X == value;
    var foreground = IsCurrentColumn ? "black" : color;
    var background = IsCurrentColumn ? color : "default";
    return new Markup($"[{foreground} on {background}]{value:X2}[/]");
  }
  public static Grid CreateDiffView()
  {
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
