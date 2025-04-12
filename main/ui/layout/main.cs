using main.config;
using main.memory;
using main.model;
using main.ui.keyhandler;

using Spectre.Console;

namespace main.ui.layout;

public interface IUi
{
  Layout CreateLayout(IConfig config, IInput input);
}

public class Ui(ICurrentConfig config, model.ICursor cursor, ISnapShot snapShot, IFocus focus, IMode mode, ISelectView selectView) : IUi
{
  private BoxBorder BorderStyle => BoxBorder.Rounded;
  private Color GetBorderColor(InputMode[]? activeModes = null, InputMode[]? inactiveModes = null)
  {
    var defaultColor = Color.Default;
    var activeColor = Color.Green;
    if (activeModes != null)
    {
      return activeModes.Contains(mode.InputMode) ? activeColor : defaultColor;
    }
    if (inactiveModes != null)
    {
      return inactiveModes.Contains(mode.InputMode) ? defaultColor : activeColor;
    }
    return defaultColor;
  }
  private readonly MainGrid mainGrid = new(config, cursor, snapShot, focus);
  public Layout CreateLayout(IConfig config, IInput input)
  {
    // Create the layout
    if (mode.InputMode == InputMode.Help)
    {
      return new KeymapView(input, selectView).View;
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
      .Border(BorderStyle)
      .Expand());
    layout["Main"]["Right"]["Top"].Update(
      new Panel(Align.Center(
        mainGrid.CursorInfoView,
        VerticalAlignment.Middle
      ))
      .Border(BorderStyle)
      .BorderColor(GetBorderColor())
      .Expand()
    );
    layout["Main"]["Right"]["Bottom"].Update(
      new Panel(Align.Center(
        mode.InputMode == InputMode.NewValue
          ? new Markup($"[red]{input.InputBuffer}[/]")
          : new Markup($"[green]shmphin[/]"),
        VerticalAlignment.Middle
      ))
      .Border(BorderStyle)
      .BorderColor(GetBorderColor())
      .Expand()
    );
    layout["Main"]["Left"].Update(
      new Panel(Align.Center(
        mainGrid.CreateDiffView(),
        VerticalAlignment.Middle
      ))
      .Border(BorderStyle)
      .BorderColor(GetBorderColor(activeModes: [InputMode.Normal]))
      .Expand()
    );
    layout["Footer"].Update(
      new Panel(Align.Center(
        Prompt.ShowInput(input.InputBuffer, mode.InputMode),
        VerticalAlignment.Middle
      ))
      .Border(BorderStyle)
      .BorderColor(GetBorderColor(inactiveModes: [InputMode.Normal]))
      .Expand()
    );
    return layout;
  }
}
