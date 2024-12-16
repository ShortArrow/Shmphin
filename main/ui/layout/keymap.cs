using Spectre.Console;
namespace main.ui.layout;
public class KeymapView(Mode mode)
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
      var keymap = new KeyMaps();
      foreach (var item in keymap.list.Select((item, index)=> new{item,index}))
      {
        var row = mode.SelectView.SelectedRow;
        var selectionStatus = row == item.index ? " > " : "   ";
        grid.AddRow([
          new Text(selectionStatus, new Style(Color.Pink1)).LeftJustified(),
          new Text(item.item.Key).RightJustified(),
          new Text(item.item.Value).LeftJustified(),
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
