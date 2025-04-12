using System.Text;

using main.ui.keyhandler;
namespace main.ui.mode;
public class Normal(IKeyMap map) : IUIMode
{
  public IKeyMap Map => map;
  public (Action, InputMode?) SelectAction(InputMode mode, StringBuilder inputBuffer, ConsoleKeyInfo keyInfo)
  {
    return SelectAction(keyInfo);
  }
  public (Action, InputMode?) SelectAction(ConsoleKeyInfo keyInfo)
  {
    map.List.TryGetValue(keyInfo.Key.ToString(), out var key);
    if (key == null)
    {
      map.List.TryGetValue(keyInfo.KeyChar.ToString(), out var charKey);
      if (charKey == null)
      {
        return (() => { }, null);
      }
      return (charKey.Execute, null);
    }
    return (key.Execute, null);
  }
}
