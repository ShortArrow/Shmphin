using System.Text;

using main.operation;
using main.ui.keyhandler;

namespace main.ui.mode;

public class CommandHandler(Func<string, IOperation> parse) : IUIMode
{
  public IKeyMap Map => new KeyMap([]);
  public (Action, InputMode?) SelectAction(ConsoleKeyInfo keyInfo)
  {
    throw new NotSupportedException();
  }
  public (Action, InputMode?) SelectAction(InputMode mode, StringBuilder inputBuffer, ConsoleKeyInfo keyInfo)
  {
    void Escape()
    {
      inputBuffer.Clear();
    }
    void Backspace()
    {
      if (inputBuffer.Length > 0)
        inputBuffer.Remove(inputBuffer.Length - 1, 1);
    }
    void Execute()
    {
      string inputString = inputBuffer.ToString();
      var operation = parse.Invoke(inputString);
      inputBuffer.Clear();
      operation.Execute();
    }
    void Append()
    {
      inputBuffer.Append(keyInfo.KeyChar);
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
