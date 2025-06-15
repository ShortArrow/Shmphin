using Spectre.Console;
using main.model;

namespace main.ui.layout;
public class CursorInfo
{
  private readonly ICursor _cursor;
  private readonly MainGrid _mainGrid;
  private readonly IFocus _focus;
  private readonly Func<uint, string> _formatAddress;

  public CursorInfo(ICursor cursor, MainGrid mainGrid, IFocus focus, Func<uint, string> FormatAddress)
  {
    _cursor = cursor;
    _mainGrid = mainGrid;
    _focus = focus;
    _formatAddress = FormatAddress;
  }

  public Dictionary<string, string> GetViewData()
  {
    _mainGrid.Matrix.Update(); // It's important this is called if data relies on updated matrix state
    var index = _cursor.GetIndex() ?? 0;
    var address = _formatAddress(index);
    var dict = new Dictionary<string, string>{
      {"viewportWidth", $"{_mainGrid.ViewportWidth}"},
      {"viewportHeight", $"{_mainGrid.ViewportHeight}"},
      {"gridWidth", $"{_mainGrid.Matrix.Width}"},
      {"gridHeight", $"{_mainGrid.Matrix.Height}"},
      {"x", $"{_cursor.X}"},
      {"y", $"{_cursor.Y}"},
      {"byteIndex", $"{index}"},
      {"wordIndex", $"{index / 2}"},
      {"BeforeValue", $"{_mainGrid.Matrix.GetCell(_cursor.X, _cursor.Y).BeforeValue}"},
      {"CurrentValue", $"{_mainGrid.Matrix.GetCell(_cursor.X, _cursor.Y).CurrentValue}"},
      {"Address", $"{address}"},
      {"focus", $"{_focus.TargetPanel}"}
    };
    return dict;
  }

  public Grid CreateCursorView()
  {
    var grid = new Grid();
    grid.AddColumns(2);
    grid.AddRow(new Markup($"[green bold]Name[/]"), new Markup($"[red bold]Value[/]"));

    var data = GetViewData();
    foreach (var item in data)
    {
      grid.AddRow(new Text(item.Key), new Text(item.Value).RightJustified());
    }
    return grid;
  }
}
