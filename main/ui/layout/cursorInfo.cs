using Spectre.Console;
using main.model;

namespace main.ui;
public class CursorInfo(Cursor cursor, Matrix matrix, Focus focus, Func<uint, string> FormatAddress)
{
  public Grid CreateCursorView()
  {
    matrix.Update();
    var grid = new Grid();
    var index = cursor.GetIndex() ?? 0;

    grid.AddColumns(2);
    grid.AddRow(new Markup($"[green bold]Name[/]"), new Markup($"[red bold]Value[/]"));
    var address = FormatAddress(index);
    var dict = new Dictionary<string, string>{
      {"x", $"{cursor.X}"},
      {"y", $"{cursor.Y}"},
      {"byteIndex", $"{index}"},
      {"wordIndex", $"{index / 2}"},
      {"BeforeValue", $"{matrix.GetCell(cursor.X, cursor.Y).BeforeValue}"},
      {"CurrentValue", $"{matrix.GetCell(cursor.X, cursor.Y).CurrentValue}"},
      {"Address", $"{address}"},
      {"gridWidth", $"{matrix.Width}"},
      {"gridHeight", $"{matrix.Height}"},
      {"focus", $"{focus.TargetPanel}"}
    };
    foreach (var item in dict)
    {
      grid.AddRow(new Text(item.Key), new Text(item.Value).RightJustified());
    }
    return grid;
  }
}
