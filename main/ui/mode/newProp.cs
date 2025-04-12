using System.Text;

using main.ui.keyhandler;

namespace main.ui.mode;
public class NewPropHandler<T>(Func<string, T> parse, TaskCompletionSource<T>? tcs) : IUIMode
{
  public IKeyMap Map => new KeyMap([]);
  public (Action, InputMode?) SelectAction(ConsoleKeyInfo keyInfo)
  {
    throw new NotSupportedException();
  }
  public (Action, InputMode?) SelectAction(InputMode mode, StringBuilder inputBuffer, ConsoleKeyInfo keyInfo)
  {
    void Execute()
    {
      string inputString = inputBuffer.ToString();
      var result = parse.Invoke(inputString);
      inputBuffer.Clear();
      tcs?.SetResult(result);
    }
    void Backspace()
    {
      if (inputBuffer.Length > 0)
        inputBuffer.Remove(inputBuffer.Length - 1, 1);
    }
    void Append()
    {
      inputBuffer.Append(keyInfo.KeyChar);
    }
    return keyInfo.Key switch
    {
      ConsoleKey.Enter => (Execute, InputMode.Normal),
      ConsoleKey.Backspace => (Backspace, mode),
      _ => (Append, mode)
    };
  }
}
