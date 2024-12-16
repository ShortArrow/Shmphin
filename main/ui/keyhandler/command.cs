using System.Text;

using main.operation;

namespace main.ui.mode;
public class CommandHandler(Func<string, IOperation> parse, InputMode mode, StringBuilder inputBuffer)
{
  public Action Invoke(ConsoleKeyInfo keyInfo)
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
