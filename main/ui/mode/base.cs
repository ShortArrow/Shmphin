using System.Text;

using main.ui.keyhandler;

namespace main.ui.mode;
public interface IUIMode
{
  (Action, InputMode?) SelectAction(InputMode mode, StringBuilder inputBuffer, ConsoleKeyInfo keyInfo);
  (Action, InputMode?) SelectAction(ConsoleKeyInfo keyInfo);
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
