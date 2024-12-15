using main.config;
using main.memory;

using Spectre.Console;

namespace main.ui.layout;

public class Ui(IConfig config, model.Cursor cursor, SnapShot snapShot, Focus focus, Mode mode)
{
  private readonly MainGrid mainGrid = new(config, cursor, snapShot, focus);
  public Layout CreateLayout(IConfig config, Input input)
  {
    var IsExMode = mode.InputMode == InputMode.Ex;
    var IsNewValueMode = mode.InputMode == InputMode.NewValue;
    // Create the layout
    if(mode.InputMode == InputMode.Help){
      return new KeymapView(mode).View;
    }
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
        new Markup($"[blue]{config.SharedMemoryName}[/]"),
        VerticalAlignment.Middle
      ))
      .Border(BoxBorder.Rounded)
      .Expand());
    layout["Main"]["Right"]["Top"].Update(
      new Panel(Align.Center(
        mainGrid.CreateCursorView(),
        VerticalAlignment.Middle
      ))
      .Border(BoxBorder.Rounded)
      .BorderColor(IsExMode ? Color.Default : focus.TargetPanel == TargetPanel.Right ? Color.Green : Color.Default)
      .Expand()
    );
    layout["Main"]["Right"]["Bottom"].Update(
      new Panel(Align.Center(
        IsNewValueMode
          ? new Markup($"[red]{input.InputBuffer}[/]")
          : new Markup($"[green]shmphin[/]"),
        VerticalAlignment.Middle
      ))
      .Border(BoxBorder.Rounded)
      .BorderColor(IsExMode ? Color.Default : focus.TargetPanel == TargetPanel.Left ? Color.Green : Color.Default)
      .Expand()
    );
    layout["Main"]["Left"].Update(
      new Panel(Align.Center(
        mainGrid.CreateDiffView(),
        VerticalAlignment.Middle
      ))
      .Border(BoxBorder.Rounded)
      .BorderColor(IsExMode ? Color.Default : focus.TargetPanel == TargetPanel.Left ? Color.Green : Color.Default)
      .Expand()
    );
    layout["Footer"].Update(
      new Panel(Align.Center(
        Prompt.ShowInput(input.InputBuffer, mode.InputMode),
        VerticalAlignment.Middle
      ))
      .Border(BoxBorder.Rounded)
      .BorderColor(IsExMode ? Color.Red : Color.Default)
      .Expand()
    );
    return layout;
  }
}
