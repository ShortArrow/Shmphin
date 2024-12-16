using System.Text;

namespace main.ui.mode;
public class NewPropHandler<T>(Func<string, T> parse, TaskCompletionSource<T>? tcs) : IUIMode
{
  public IKeyMap Map => new KeyMap([]);
  public Action SelectAction(ConsoleKeyInfo keyInfo)
  {
    throw new NotSupportedException();
  }
  public Action SelectAction(Mode mode, StringBuilder inputBuffer, ConsoleKeyInfo keyInfo)
  {
    return keyInfo.Key switch
    {
      ConsoleKey.Enter => () =>
      {
        mode.InputMode = InputMode.Normal;
        string inputString = inputBuffer.ToString();
        var result = parse.Invoke(inputString);
        inputBuffer.Clear();
        tcs?.SetResult(result);
      }
      ,
      ConsoleKey.Backspace => () =>
      {
        if (inputBuffer.Length > 0)
          inputBuffer.Remove(inputBuffer.Length - 1, 1);
      }
      ,
      _ => () => inputBuffer.Append(keyInfo.KeyChar)
    };
  }
}
