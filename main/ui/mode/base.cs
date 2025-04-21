using System.Text;

using main.operation;
using main.ui.keyhandler;

namespace main.ui.mode;
public interface IUIMode
{
  (Func<Task>, InputMode?) SelectAction(InputMode mode, StringBuilder inputBuffer, ConsoleKeyInfo keyInfo);
  (Func<Task>, InputMode?) SelectAction(ConsoleKeyInfo keyInfo);
  IKeyMap Map { get; }
}

public class KeyAction : IKeyAction
{
  public KeyAction(string defaultKey, string description, IOperation operation)
  {
    this.defaultKey = defaultKey;
    this.description = description;
    this.action = operation.Execute;
  }
  public KeyAction(string defaultKey, string description, Func<Task> action)
  {

    this.defaultKey = defaultKey;
    this.description = description;
    this.action = action;
  }

  private readonly string defaultKey;
  private readonly string description;
  private readonly Func<Task> action;

  public string DefaultKey { get => defaultKey; }
  public string Description { get => description; }
  public Task Execute() => action();
}

public class KeyMap(Dictionary<string, IKeyAction> dict) : IKeyMap
{
  public Dictionary<string, IKeyAction> List { get => dict; }
}
