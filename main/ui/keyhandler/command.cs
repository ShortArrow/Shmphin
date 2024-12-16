using System.Data;
using System.Text;

using main.operation;

namespace main.ui.mode;
public class CommandHandler(Func<string, IOperation> parse) : IUIMode
{
  public IKeyMap Map => new KeyMap([]);
  public Action SelectAction(ConsoleKeyInfo keyInfo)
  {
    throw new NotSupportedException();
  }
  public Action SelectAction(InputMode mode, StringBuilder inputBuffer, ConsoleKeyInfo keyInfo)
  {
    return keyInfo.Key switch
    {
      ConsoleKey.Enter => () =>
      {
        mode = InputMode.Normal;
        string inputString = inputBuffer.ToString();
        var operation = parse.Invoke(inputString);
        inputBuffer.Clear();
        operation.Execute();
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
