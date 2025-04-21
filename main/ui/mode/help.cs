using System.Text;

using main.ui.keyhandler;
namespace main.ui.mode;
public class HelpViewHandler(IKeyMap map) : IUIMode
{
  public IKeyMap Map => map;
  public (Func<Task>, InputMode?) SelectAction(InputMode mode, StringBuilder inputBuffer, ConsoleKeyInfo keyInfo)
  {
    return SelectAction(keyInfo);
  }
  public (Func<Task>, InputMode?) SelectAction(ConsoleKeyInfo keyInfo)
  {
    map.List.TryGetValue(keyInfo.Key.ToString(), out var key);
    if (key == null)
    {
      map.List.TryGetValue(keyInfo.KeyChar.ToString(), out var keyChar);
      if (keyChar == null)
      {
        return (() => Task.CompletedTask, null);
      }
      return (keyChar.Execute, null);
    }
    return (key.Execute, null);
  }
}
