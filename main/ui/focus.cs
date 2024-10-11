
enum TargetPanel
{
  Right,
  Left,
}

class Focus
{
  private static TargetPanel _targetPanel = TargetPanel.Left;
  public static TargetPanel TargetPanel
  {
    get => _targetPanel;
    set => _targetPanel = value;
  }
  public static void ChangeFocus()
  {
    _targetPanel = _targetPanel switch
    {
      TargetPanel.Left => TargetPanel.Right,
      TargetPanel.Right => TargetPanel.Left,
      _ => throw new Exception("Invalid TargetPanel value"),
    };
  }
}