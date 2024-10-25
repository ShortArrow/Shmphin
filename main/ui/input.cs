using System.Diagnostics;
using System.Text;
using Spectre.Console;
using main.operation;
using main.config;

namespace main.ui;

public enum InputMode
{
  Normal,
  Ex,
  NewValue,
  NewCellSize,
  NewColumnsLength
}

public class Input
{
  private readonly Operations operations;
  private readonly Parser parser;
  public Input(IConfig config, model.Cursor cursor)
  {
    operations = new(config, cursor);
    parser = new Parser(operations);
  }
  private InputMode mode = InputMode.Normal;
  public InputMode Mode
  {
    get => mode;
    set => mode = value;
  }
  private readonly CancellationTokenSource cts = new();
  public bool IsCancellationRequested => cts.Token.IsCancellationRequested;
  private readonly StringBuilder inputBuffer = new();
  public string InputBuffer => inputBuffer.ToString();
  private TaskCompletionSource<byte[]>? newValueTcs;
  private TaskCompletionSource<uint>? newCellSizeTcs;
  private TaskCompletionSource<uint>? newColumnsLengthTcs;
  public static string GetSharedMemoryName()
  {
    return AnsiConsole.Prompt(
      new TextPrompt<string>("Enter the shared memory name:")
    );
  }
  internal Task<byte[]> NewValue()
  {
    mode = InputMode.NewValue;
    Debug.WriteLine("New value mode");
    newValueTcs = new TaskCompletionSource<byte[]>();
    return newValueTcs.Task;
  }
  internal Task<uint> NewCellSize()
  {
    mode = InputMode.NewCellSize;
    Debug.WriteLine("New cell size mode");
    newCellSizeTcs = new TaskCompletionSource<uint>();
    return newCellSizeTcs.Task;
  }
  internal Task<uint> NewColumnsLength()
  {
    mode = InputMode.NewColumnsLength;
    Debug.WriteLine("New columns length mode");
    newColumnsLengthTcs = new TaskCompletionSource<uint>();
    return newColumnsLengthTcs.Task;
  }

  public void InputLoop()
  {
    while (!cts.Token.IsCancellationRequested)
    {
      var key = Console.ReadKey(true);
      switch (mode)
      {
        case InputMode.Ex:
          if (key.Key == ConsoleKey.Enter)
          {
            mode = InputMode.Normal;
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
            mode = InputMode.Normal;
            var inputString = inputBuffer.ToString();
            var result = Parse.NewValue(inputString);
            inputBuffer.Clear();
            newValueTcs?.SetResult(result);
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
            mode = InputMode.Normal;
            var inputString = inputBuffer.ToString();
            var result = Parse.CellSize(inputString);
            inputBuffer.Clear();
            newCellSizeTcs?.SetResult(result);
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
            mode = InputMode.Normal;
            var inputString = inputBuffer.ToString();
            var result = Parse.ColumnsLength(inputString);
            inputBuffer.Clear();
            newColumnsLengthTcs?.SetResult(result);
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
          if (Parse.KeyCheck(key, "q")) { cts.Cancel(); }
          else if (Parse.KeyCheck(key, ":"))
          {
            mode = InputMode.Ex;  // Enter Ex mode
            inputBuffer.Clear();
            inputBuffer.Append('>');
          }
          else if (Parse.KeyCheck(key, "u")) { operations.UpdateMemory.Execute(); }
          else if (Parse.KeyCheck(key, "h")) { operations.Left.Execute(); }
          else if (Parse.KeyCheck(key, "j")) { operations.Down.Execute(); }
          else if (Parse.KeyCheck(key, "k")) { operations.Up.Execute(); }
          else if (Parse.KeyCheck(key, "l")) { operations.Right.Execute(); }
          // else if (Parse.KeyCheck(key, "c")) { operations.ChangeMemory.Execute(); }
          // else if (Parse.KeyCheck(key, "?")) { operations.Help.Execute(); }
          // else if (Parse.KeyCheck(key, "/")) { operations.Search.Execute(); }
          else if (key.Key == ConsoleKey.Tab) { Focus.ChangeFocus(); }
          break;
        default:
          break;
      }
    }
  }
}
