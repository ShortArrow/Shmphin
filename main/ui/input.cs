using System.Diagnostics;
using System.Text;
using Spectre.Console;
using main.operation;

namespace main.ui;

public enum InputMode
{
  Normal,
  Ex,
  NewValue
}

public class Input
{
  private static InputMode mode = InputMode.Normal;
  public static InputMode Mode
  {
    get => mode;
    set => mode = value;
  }
  private static readonly CancellationTokenSource cts = new();
  public static bool IsCancellationRequested => cts.Token.IsCancellationRequested;
  private static readonly StringBuilder inputBuffer = new();
  public static string InputBuffer => inputBuffer.ToString();
  private static TaskCompletionSource<byte[]>? newValueTcs;
  public static string GetSharedMemoryName()
  {
    return AnsiConsole.Prompt(
      new TextPrompt<string>("Enter the shared memory name:")
    );
  }
  internal static Task<byte[]> NewValue()
  {
    mode = InputMode.NewValue;
    Debug.WriteLine("New value mode");
    newValueTcs = new TaskCompletionSource<byte[]>();
    return newValueTcs.Task;
  }
  public static bool KeyCheck(ConsoleKeyInfo keyInfo, string name)
  {
    return keyInfo
      .KeyChar
      .ToString()
      .Equals(name, StringComparison.CurrentCultureIgnoreCase);
  }
  public static byte[] ParseNewValue(string inputString)
  {
    List<byte> byteList = [];

    if (ulong.TryParse(inputString, out ulong value))
    {
      if (value <= byte.MaxValue)
        byteList.Add((byte)value);
      else
        throw new Exception($"Invalid input {inputString} (value is too large)");

    }
    else
    {
      throw new Exception($"Invalid input {inputString}");
    }

    return [.. byteList];
  }
  public static void InputLoop()
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
            // Execute the command
            inputBuffer.Clear();
            inputBuffer.Append("Command executed.");
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
            var result = ParseNewValue(inputString);
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
        case InputMode.Normal:
          Debug.WriteLine($"nomal mode: {key.KeyChar}");
          if (KeyCheck(key, "q")) { cts.Cancel(); }
          else if (KeyCheck(key, ":"))
          {
            mode = InputMode.Ex;  // Enter Ex mode
            inputBuffer.Clear();
            inputBuffer.Append('>');
          }
          else if (KeyCheck(key, "u")) { Operation.UpdateMemory.Execute(); }
          else if (KeyCheck(key, "h")) { Cursor.MoveLeft(); }
          else if (KeyCheck(key, "j")) { Cursor.MoveDown(); }
          else if (KeyCheck(key, "k")) { Cursor.MoveUp(); }
          else if (KeyCheck(key, "l")) { Cursor.MoveRight(); }
          else if (KeyCheck(key, "m")) { GridMode.ChangeGridType(); }
          else if (KeyCheck(key, "c")) { Operation.ChangeMemory.Execute(); }
          else if (key.Key == ConsoleKey.Tab) { Focus.ChangeFocus(); }
          break;
        default:
          break;
      }
    }
  }
}