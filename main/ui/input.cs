using System.Diagnostics;
using System.Text;
using main.operation;
using main.ui.keyhandler;

namespace main.ui;

public enum InputMode
{
  Normal,
  Ex,
  NewValue,
  NewCellSize,
  NewColumnsLength
}

public class Input(Operations operations, Mode mode)
{
  private readonly StringBuilder inputBuffer = new();
  public string InputBuffer => inputBuffer.ToString();
  private readonly Parser parser = new(operations);
  public void InputLoop()
  {
    while (!mode.cts.Token.IsCancellationRequested)
    {
      var key = Console.ReadKey(true);
      switch (mode.InputMode)
      {
        case InputMode.Ex:
          if (key.Key == ConsoleKey.Enter)
          {
            mode.InputMode = InputMode.Normal;
            var command = inputBuffer.ToString()[1..]; // Remove the first character '>'
            var operation = parser.Parse(command);
            operation.Execute();
            inputBuffer.Clear();
            Debug.WriteLine($"Command: [{operation.Name}]");
          }
          else if (key.Key == ConsoleKey.Backspace)
          {
            if (inputBuffer.Length > 0)
              inputBuffer.Remove(inputBuffer.Length - 1, 1);
          }
          else
          {
            inputBuffer.Append(key.KeyChar);
          }
          break;
        case InputMode.NewValue:
          if (key.Key == ConsoleKey.Enter)
          {
            mode.InputMode = InputMode.Normal;
            var inputString = inputBuffer.ToString();
            var result = Parse.NewValue(inputString);
            inputBuffer.Clear();
            mode.newValueTcs?.SetResult(result);
          }
          else if (key.Key == ConsoleKey.Backspace)
          {
            if (inputBuffer.Length > 0)
              inputBuffer.Remove(inputBuffer.Length - 1, 1);
          }
          else
          {
            Debug.WriteLine($"New value mode: {key.KeyChar}");
            inputBuffer.Append(key.KeyChar);
          }
          break;
        case InputMode.NewCellSize:
          if (key.Key == ConsoleKey.Enter)
          {
            mode.InputMode = InputMode.Normal;
            var inputString = inputBuffer.ToString();
            var result = Parse.CellSize(inputString);
            inputBuffer.Clear();
            mode.newCellSizeTcs?.SetResult(result);
          }
          else if (key.Key == ConsoleKey.Backspace)
          {
            if (inputBuffer.Length > 0)
              inputBuffer.Remove(inputBuffer.Length - 1, 1);
          }
          else
          {
            Debug.WriteLine($"New value mode: {key.KeyChar}");
            inputBuffer.Append(key.KeyChar);
          }
          break;
        case InputMode.NewColumnsLength:
          if (key.Key == ConsoleKey.Enter)
          {
            mode.InputMode = InputMode.Normal;
            var inputString = inputBuffer.ToString();
            var result = Parse.ColumnsLength(inputString);
            inputBuffer.Clear();
            mode.newColumnsLengthTcs?.SetResult(result);
          }
          else if (key.Key == ConsoleKey.Backspace)
          {
            if (inputBuffer.Length > 0)
              inputBuffer.Remove(inputBuffer.Length - 1, 1);
          }
          else
          {
            Debug.WriteLine($"New value mode: {key.KeyChar}");
            inputBuffer.Append(key.KeyChar);
          }
          break;
        case InputMode.Normal:
          Debug.WriteLine($"nomal mode: {key.KeyChar}");
          var handler = new NormalKeyEvent(operations);
          handler.Handling(key).Execute();
          break;
        default:
          break;
      }
    }
  }
}
