using System.Text;
namespace main.ui.mode;
public class HelpViewHandler(IKeyMap map) : IUIMode
{
  public IKeyMap Map => map;
  public Action SelectAction(InputMode mode, StringBuilder inputBuffer, ConsoleKeyInfo keyInfo)
  {
    return SelectAction(keyInfo);
  }
  public Action SelectAction(ConsoleKeyInfo keyInfo)
  {
    map.List.TryGetValue(keyInfo.Key.ToString(), out var key);
    if (key == null)
    {
      map.List.TryGetValue(keyInfo.KeyChar.ToString(), out var keyChar);
      if (keyChar == null)
      {
        return () => { };
      }
      return keyChar.Execute;
    }
    return key.Execute;
  }
}
