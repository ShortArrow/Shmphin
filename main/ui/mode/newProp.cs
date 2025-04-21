using System.Text;

using main.ui.keyhandler;

namespace main.ui.mode;
public class NewPropHandler<T>(Func<string, T> parse, TaskCompletionSource<T>? tcs) : IUIMode
{
  public IKeyMap Map => new KeyMap([]);
  public (Func<Task>, InputMode?) SelectAction(ConsoleKeyInfo keyInfo)
  {
    throw new NotSupportedException();
  }
  public (Func<Task>, InputMode?) SelectAction(InputMode mode, StringBuilder inputBuffer, ConsoleKeyInfo keyInfo)
  {
    Task Execute()
    {
      string inputString = inputBuffer.ToString();
      var result = parse.Invoke(inputString);
      inputBuffer.Clear();
      tcs?.SetResult(result);
      return Task.CompletedTask;
    }
    Task Backspace()
    {
      if (inputBuffer.Length > 0)
        inputBuffer.Remove(inputBuffer.Length - 1, 1);
      return Task.CompletedTask;
    }
    Task Append()
    {
      inputBuffer.Append(keyInfo.KeyChar);
      return Task.CompletedTask;
    }
    return keyInfo.Key switch
    {
      ConsoleKey.Enter => (Execute, InputMode.Normal),
      ConsoleKey.Backspace => (Backspace, mode),
      _ => (Append, mode)
    };
  }
}
