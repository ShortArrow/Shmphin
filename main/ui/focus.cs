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
  }
  private readonly ChangeFocus changeFocus;
  public Focus()
  {
    changeFocus = new(ToggleFocus);
  }
  public void ToggleFocus()
  {
    targetPanel = targetPanel switch
    {
      TargetPanel.Left => TargetPanel.Right,
      TargetPanel.Right => TargetPanel.Left,
      _ => throw new Exception("Invalid TargetPanel value"),
    };
  }
  public ChangeFocus ChangeFocus => changeFocus;
}

public class ChangeFocus(Action action) : Operation
{
  public override string Name => "change_focus";
  public override void Execute()
  {
    action.Invoke();
  }
}
