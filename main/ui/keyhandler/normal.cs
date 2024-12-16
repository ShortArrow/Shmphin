using main.operation;

namespace main.ui.keyhandler;
public class Normal(Operations operations) : IKeyHandler
{
  private readonly KeyMap map = new(
    new Dictionary<string, IKeyAction> {
      { "h", new KeyAction("h", "move left", operations.Left.Execute) },
      { "j", new KeyAction("j", "move down", operations.Down.Execute) },
      { "k", new KeyAction("k", "move up", operations.Up.Execute) },
      { "l", new KeyAction("l", "move right", operations.Right.Execute) },
      { "c", new KeyAction("c", "change memory", operations.ChangeMemory.Execute) },
      { "q", new KeyAction("q", "quit", operations.Quit.Execute) },
      { "s", new KeyAction("s", "cell", operations.Cell.Execute) },
      { "n", new KeyAction("n", "name", operations.Name.Execute) },
      { ":", new KeyAction(":", "ex command", operations.ExCommand.Execute) },
      { "?", new KeyAction("?", "help", operations.Help.Execute) },
      { "/", new KeyAction("/", "search", operations.Search.Execute) },
      { "Tab", new KeyAction("Tab", "change focus", operations.ChangeFocus.Execute) }
    }
  );
  public IKeyMap Map => map;
  public Action Invoke(ConsoleKeyInfo keyInfo)
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
