using Spectre.Console;
namespace main.ui.layout;
public class KeymapView
{
  public Layout View
  {
    get
    {
      var grid = new Grid();
      grid.AddColumn();
      grid.AddColumn();
      grid.AddColumn();

      grid.AddRow([
        new Text("*", new Style(Color.Pink1)).LeftJustified(),
        new Text("Key", new Style(Color.Blue)).RightJustified(),
        new Text("Action", new Style(Color.Red)).LeftJustified(),
      ]);
      var keymap = new KeyMap();
      foreach (var item in keymap.list)
      {
        grid.AddRow([
          new Text("", new Style(Color.Pink1)).LeftJustified(),
          new Text(item.Key).RightJustified(),
          new Text(item.Value).LeftJustified(),
        ]);
      }
      var layout = new Layout("Root");
      layout["Root"].Update(
        new Panel(Align.Center(grid))
      );
      return layout;
    }
  }
}
