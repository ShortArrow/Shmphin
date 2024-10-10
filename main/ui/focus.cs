
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
    switch (_targetPanel)
    {
      case TargetPanel.Left:
        _targetPanel = TargetPanel.Right;
        break;
      case TargetPanel.Right:
        _targetPanel = TargetPanel.Left;
        break;
      default:
        throw new Exception("Invalid TargetPanel value");
    }
  }
}