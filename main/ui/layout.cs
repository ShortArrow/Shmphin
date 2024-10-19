using main.model;

using Spectre.Console;

namespace main.ui;

class Ui
{
  public static Layout CreateLayout(string message)
  {
    var IsExMode = Input.Mode == InputMode.Ex;
    var IsNewValueMode = Input.Mode == InputMode.NewValue;
    // Create the layout
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
        new Markup($"[blue]{memory.Params.SharedMemoryName}[/]"),
        VerticalAlignment.Middle
      ))
      .Expand());
    layout["Main"]["Right"]["Top"].Update(
      new Panel(Align.Center(
        MainGrid.CreateCursorView(),
        VerticalAlignment.Middle
      ))
      .BorderColor(IsExMode ? Color.Default : Focus.TargetPanel == TargetPanel.Right ? Color.Green : Color.Default)
      .Expand()
    );
    layout["Main"]["Right"]["Bottom"].Update(
      new Panel(Align.Center(
        IsNewValueMode
          ? new Markup($"[red]{Input.InputBuffer}[/]")
          : new Markup($"[green]shmphin[/]"),
        VerticalAlignment.Middle
      ))
      .BorderColor(IsExMode ? Color.Default : Focus.TargetPanel == TargetPanel.Left ? Color.Green : Color.Default)
      .Expand()
    );
    layout["Main"]["Left"].Update(
      new Panel(Align.Center(
        MainGrid.CreateDiffView(),
        VerticalAlignment.Middle
      ))
      .BorderColor(IsExMode ? Color.Default : Focus.TargetPanel == TargetPanel.Left ? Color.Green : Color.Default)
      .Expand()
    );
    layout["Footer"].Update(
      new Panel(Align.Center(
        new Markup($"[green]{Input.InputBuffer}[/]"),
        VerticalAlignment.Middle
      ))
      .BorderColor(IsExMode ? Color.Red : Color.Default)
      .Expand()
    );
    return layout;
  }
}
