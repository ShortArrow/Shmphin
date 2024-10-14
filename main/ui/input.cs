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
    if (string.IsNullOrEmpty(inputString))
    {
      throw new Exception($"Invalid input {inputString} (input should be a non-empty string)");
    }

    // 文字列の長さが奇数の場合、先頭に '0' を追加
    if (inputString.Length % 2 != 0)
    {
      inputString = "0" + inputString;
    }

    List<byte> byteList = [];

    for (int i = 0; i < inputString.Length; i += 2)
    {
      string hexByte = inputString.Substring(i, 2);
      if (byte.TryParse(hexByte, System.Globalization.NumberStyles.HexNumber, null, out byte value))
      {
        byteList.Add(value);
      }
      else
      {
        throw new Exception($"Invalid input {inputString} (contains non-hex characters)");
      }
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
          else if (KeyCheck(key, "?")) { Operation.ShowHelp.Execute(); }
          else if (KeyCheck(key, "/")) { Operation.Search.Execute(); }
          else if (key.Key == ConsoleKey.Tab) { Focus.ChangeFocus(); }
          break;
        default:
          break;
      }
    }
  }
}