
using System.Diagnostics;
using System.Text;
using Spectre.Console;
using main.operation;

namespace main.ui;

class Input
{
  public static CancellationTokenSource cts = new();
  public static StringBuilder inputBuffer = new();
  public static bool IsNewValueMode = false;
  public static string GetSharedMemoryName()
  {
    return AnsiConsole.Prompt(
      new TextPrompt<string>("Enter the shared memory name:")
    );
  }
  internal static Task<byte[]> NewValue()
  {
    return Task.Run(async () =>
    {
      IsNewValueMode = true;
      inputBuffer.Clear();
      while (IsNewValueMode)
      {
        await Task.Delay(100);
      }
      return Encoding.ASCII.GetBytes(inputBuffer.ToString());
    });
  }
  public static bool KeyCheck(ConsoleKeyInfo keyInfo, string name)
  {
    return keyInfo
      .KeyChar
      .ToString()
      .Equals(name, StringComparison.CurrentCultureIgnoreCase);
  }
  public static void InputLoop()
  {
    while (!cts.Token.IsCancellationRequested)
    {
      var key = Console.ReadKey(true);
      if (Ui.IsExMode)
      {
        if (key.Key == ConsoleKey.Enter)
        {
          Ui.IsExMode = false;
          // Execute the command
          inputBuffer.Clear();
          inputBuffer.Append("Command executed.");
        }
        else if (key.Key == ConsoleKey.Backspace)
        {
          inputBuffer.Remove(inputBuffer.Length - 1, 1);
        }
        else
        {
          inputBuffer.Append(key.KeyChar);
        }
      }
      else
      {
        Debug.WriteLine($"nomal mode: {key.KeyChar}");
        if (KeyCheck(key, "q"))
        {
          cts.Cancel(); // Request cancellation
        }
        else if (KeyCheck(key, "u"))
        {
          Operation.UpdateMemory.Execute();
        }
        else if (KeyCheck(key, ":"))
        {
          Ui.IsExMode = true;  // Enter Ex mode
          inputBuffer.Clear();
          inputBuffer.Append('>');
        }
        else if (KeyCheck(key, "h")) { Cursor.MoveLeft(); }
        else if (KeyCheck(key, "j")) { Cursor.MoveDown(); }
        else if (KeyCheck(key, "k")) { Cursor.MoveUp(); }
        else if (KeyCheck(key, "l")) { Cursor.MoveRight(); }
        else if (KeyCheck(key, "m")) { GridMode.ChangeGridType(); }
        else if (KeyCheck(key, "c")) { Operation.ChangeMemory.Execute(); }
        else if (key.Key == ConsoleKey.Tab)
        {
          Focus.ChangeFocus();
        }
      }
    }
  }
}