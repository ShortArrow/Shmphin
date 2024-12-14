using Spectre.Console;
namespace main.ui.layout;
public class KeymapView
{
  public Layout View
  {
    get
    {
      var table = new Table();
      table.AddColumn("Key");
      table.AddColumn("Action");
      var keymap = new KeyMap();
      foreach (var item in keymap.list)
      {
        table.AddRow(item.Key, item.Value);
      }
      table.Border(TableBorder.Rounded);
      table.Expand();

      var layout = new Layout("Root");
      layout["Root"].Update(
        table
      );
      return layout;
    }
  }
}
