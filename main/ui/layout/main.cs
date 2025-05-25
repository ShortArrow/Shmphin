using main.config;
using main.memory;
using main.model;
using main.ui.keyhandler;

using Spectre.Console;

namespace main.ui.layout;

public interface IUi
{
  Layout CreateLayout(IConfig config, IInput input);
  main.ui.layout.MainGrid CurrentMainGrid { get; }
}

public class Ui(ICurrentConfig config, ICursor cursor, ISnapShot snapShot, IFocus focus, IMode mode, ISelectView selectView) : IUi
{
  public MainGrid CurrentMainGrid => mainGrid;
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
  public Layout CreateLayout(IConfig config, IInput input) // The parameters config and input will not be used in this temporary version.
  {
      var layout = new Layout("Root");
      layout.Update(new Panel(new Markup("[bold green]MINIMAL UI TEST OK[/]")));
      return layout;
  }
}
