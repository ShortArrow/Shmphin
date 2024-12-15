namespace main.ui.keyhandler;
public class HelpViewHandler(Mode mode)
{
  private void Escape()
  {
    mode.InputMode = InputMode.Normal;
  }
  public Action Invoke(ConsoleKeyInfo keyInfo)
  {
    return keyInfo.Key switch
    {
      ConsoleKey.Escape => Escape,
      _ => keyInfo.KeyChar.ToString() switch
      {
        "j" => mode.SelectView.MoveDown,
        "k" => mode.SelectView.MoveUp,
        "q" => Escape,
        _ => () => { }
      }
    };
  }
}
