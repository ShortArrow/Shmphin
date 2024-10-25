using main.operation;

namespace main.ui;
public enum TargetPanel
{
  Right,
  Left,
}

public class Focus
{
  private TargetPanel targetPanel = TargetPanel.Left;
  public TargetPanel TargetPanel
  {
    get => targetPanel;
    set => targetPanel = value;
  }
  private readonly ChangeFocus changeFocus;
  public Focus()
  {
    changeFocus = new(targetPanel);
  }
  public ChangeFocus ChangeFocus => changeFocus;
}

public class ChangeFocus(TargetPanel focus) : Operation
{
  public override string Name => "change_focus";
  public override void Execute()
  {
    focus = focus switch
    {
      TargetPanel.Left => TargetPanel.Right,
      TargetPanel.Right => TargetPanel.Left,
      _ => throw new Exception("Invalid TargetPanel value"),
    };
  }
}
