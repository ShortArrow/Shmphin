namespace main.ui.keyhandler;

public interface IKeyHandler
{
  Action Invoke(ConsoleKeyInfo keyInfo);
  IKeyMap Map { get; }
}

public class KeyAction(string defaultKey, string description, Action action) : IKeyAction
{
  public string DefaultKey { get => defaultKey; }
  public string Description { get => description; }
  public void Execute() => action();
}

public class KeyMap(Dictionary<string, IKeyAction> dict) : IKeyMap
{
  public Dictionary<string, IKeyAction> List { get => dict; }
}
