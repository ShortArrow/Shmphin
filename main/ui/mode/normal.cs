using System.Text;
namespace main.ui.mode;
public class Normal(IKeyMap map) : IUIMode
{
  public IKeyMap Map => map;
  public Action SelectAction(Mode mode, StringBuilder inputBuffer, ConsoleKeyInfo keyInfo)
  {
    return SelectAction(keyInfo);
  }
  public Action SelectAction(ConsoleKeyInfo keyInfo)
  {
    map.List.TryGetValue(keyInfo.Key.ToString(), out var key);
    if (key == null)
    {
      map.List.TryGetValue(keyInfo.KeyChar.ToString(), out var charKey);
      if (charKey == null)
      {
        return () => { };
      }
      return charKey.Execute;
    }
    return key.Execute;
  }
}
