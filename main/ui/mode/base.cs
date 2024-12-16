namespace main.ui.mode;
using System.Text;

public interface IUIMode
{
  Action SelectAction(InputMode mode, StringBuilder inputBuffer, ConsoleKeyInfo keyInfo);
  Action SelectAction(ConsoleKeyInfo keyInfo);
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
