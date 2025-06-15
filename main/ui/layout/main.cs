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

public static class LayoutConstants
{
  public const uint ReservedUiHeight = 10; // Space for header (3), footer (3), borders, and padding
  public const uint MinimumViewportWidth = 40; // Minimum usable width for content display
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
    // STEP 4: Use console dimensions with padding for panels/borders
    // Instead of using matrix dimensions (which can be much larger than viewport),
    // use console size minus space for UI elements (headers, footers, borders, etc.)
    
    uint consoleHeight;
    uint consoleWidth;
    
    try
    {
      consoleHeight = (uint)Math.Max(System.Console.WindowHeight, 0);
      consoleWidth = (uint)Math.Max(System.Console.WindowWidth, 0);
    }
    catch
    {
      // Fallback if console size is not available (e.g., in tests or non-interactive environments)
      consoleHeight = 25; // Standard terminal height
      consoleWidth = 80;  // Standard terminal width
    }
    
    // Reserve space for header (3), footer (3), borders, and right panel
    // Rough estimate: left panel gets about 60% of width, 80% of available height
    var availableHeight = consoleHeight > LayoutConstants.ReservedUiHeight ? consoleHeight - LayoutConstants.ReservedUiHeight : LayoutConstants.ReservedUiHeight;
    var availableWidth = consoleWidth > LayoutConstants.MinimumViewportWidth ? (consoleWidth * 6) / 10 : LayoutConstants.MinimumViewportWidth;
    
    // Ensure matrix is updated before checking its dimensions
    // Use try-catch to handle cases where matrix can't be updated (e.g., in tests)
    try
    {
      _mainGrid.Matrix.Update();
    }
    catch
    {
      // If matrix update fails, we'll use the current matrix dimensions
      // This might happen in test environments or when config is incomplete
    }
    
    _mainGrid.SetViewportDimensions(availableHeight, availableWidth);

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
