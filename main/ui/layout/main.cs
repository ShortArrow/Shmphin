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

public class Ui : IUi
{
  private readonly ICurrentConfig _config;
  private readonly IMode _mode;
  private readonly ISelectView _selectView;
  private readonly MainGrid _mainGrid;

  public Ui(ICurrentConfig config, IMode mode, ISelectView selectView, MainGrid mainGrid)
  {
    _config = config;
    _mode = mode;
    _selectView = selectView;
    _mainGrid = mainGrid;
  }

  private BoxBorder BorderStyle => BoxBorder.Rounded;
  private Color GetBorderColor(InputMode[]? activeModes = null, InputMode[]? inactiveModes = null)
  {
    var defaultColor = Color.Default;
    var activeColor = Color.Green;
    if (activeModes != null)
    {
      return activeModes.Contains(_mode.InputMode) ? activeColor : defaultColor;
    }
    if (inactiveModes != null)
    {
      return inactiveModes.Contains(_mode.InputMode) ? defaultColor : activeColor;
    }
    return defaultColor;
  }

  public Layout CreateLayout(IConfig config, IInput input) // config parameter here is a bit redundant if _config is the same, but CreateLayout is an interface method.
  {
    // Create the layout
    if (_mode.InputMode == InputMode.Help)
    {
      return new KeymapView(input, _selectView).View;
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
        new Markup($"[blue]{_config.SharedMemoryName}[/]"), // Use injected _config
        VerticalAlignment.Middle
      ))
      .Border(BorderStyle)
      .Expand());

    // Set viewport dimensions for mainGrid.
    // STEP 1: Query Layout Region Dimensions:
    // Attempted to find a way to get character dimensions of layout["Main"]["Left"].
    // However, Spectre.Console typically resolves dimensions during the rendering pass,
    // and direct querying of pre-render dimensions for a LayoutRegion is not reliably available.
    // STEP 4: Fallback/Reporting:
    // Using fallback: full matrix dimensions. The core requirement of "actual displayable range"
    // based on panel size is not met due to this limitation.
    _mainGrid.SetViewportDimensions(_mainGrid.Matrix.Height, _mainGrid.Matrix.Width); // Use injected _mainGrid

    layout["Main"]["Right"]["Top"].Update(
      new Panel(Align.Center(
        _mainGrid.CursorInfoView, // Use injected _mainGrid
        VerticalAlignment.Middle
      ))
      .Border(BorderStyle)
      .BorderColor(GetBorderColor())
      .Expand()
    );
    layout["Main"]["Right"]["Bottom"].Update(
      new Panel(Align.Center(
        _mode.InputMode == InputMode.NewValue // Use injected _mode
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
        _mainGrid.CreateDiffView(), // Use injected _mainGrid
        VerticalAlignment.Middle
      ))
      .Border(BorderStyle)
      .BorderColor(GetBorderColor(activeModes: [InputMode.Normal]))
      .Expand()
    );
    layout["Footer"].Update(
      new Panel(Align.Center(
        Prompt.ShowInput(input.InputBuffer, _mode.InputMode), // Use injected _mode
        VerticalAlignment.Middle
      ))
      .Border(BorderStyle)
      .BorderColor(GetBorderColor(inactiveModes: [InputMode.Normal]))
      .Expand()
    );
    return layout;
  }
}
