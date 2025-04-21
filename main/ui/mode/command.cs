using System.Text;

using main.operation;
using main.ui.keyhandler;

namespace main.ui.mode;

public class CommandHandler(Func<string, IOperation> parse) : IUIMode
{
  public IKeyMap Map => new KeyMap([]);
  public (Func<Task>, InputMode?) SelectAction(ConsoleKeyInfo keyInfo)
  {
    throw new NotSupportedException();
  }
  public (Func<Task>, InputMode?) SelectAction(InputMode mode, StringBuilder inputBuffer, ConsoleKeyInfo keyInfo)
  {
    Task Escape()
    {
      inputBuffer.Clear();
      return Task.CompletedTask;
    }
    Task Backspace()
    {
      if (inputBuffer.Length > 0)
        inputBuffer.Remove(inputBuffer.Length - 1, 1);
      return Task.CompletedTask;
    }
    Task Execute()
    {
      string inputString = inputBuffer.ToString();
      var operation = parse.Invoke(inputString);
      inputBuffer.Clear();
      operation.Execute();
      return Task.CompletedTask;
    }
    Task Append()
    {
      inputBuffer.Append(keyInfo.KeyChar);
      return Task.CompletedTask;
    }
    return keyInfo.Key switch
    {
      ConsoleKey.Escape => (Escape, InputMode.Normal),
      ConsoleKey.Enter => (Execute, null),
      ConsoleKey.Backspace => (Backspace, null),
      _ => (Append, null)
    };
  }
}
