namespace main.ui.mode;
public class Normal(IKeyMap map) : IUIMode
{
  public IKeyMap Map => map;
  public Action Handler(ConsoleKeyInfo keyInfo)
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
