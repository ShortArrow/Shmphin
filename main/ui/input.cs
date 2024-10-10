
using System.Text;
using Spectre.Console;

namespace main.ui;

class Input
{
  public static TaskCompletionSource<bool> uts = new(false);
  public static CancellationTokenSource cts = new();
  public static StringBuilder inputBuffer = new();
  public static string GetSharedMemoryName()
  {
    return AnsiConsole.Prompt(
      new TextPrompt<string>("Enter the shared memory name:")
    );
  }
  public static bool KeyCheck(ConsoleKeyInfo keyInfo, string name)
  {
    return keyInfo
      .Key
      .ToString()
      .Equals(name, StringComparison.CurrentCultureIgnoreCase);
  }
  public static void InputLoop()
  {
    while (!cts.Token.IsCancellationRequested)
    {
      var key = Console.ReadKey(true);
      var name = key.KeyChar.ToString();
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
        if (KeyCheck(key, "q"))
        {
          cts.Cancel(); // Request cancellation
        }
        else if (KeyCheck(key, "u"))
        {
          uts.TrySetResult(true); // Request update SnapShot
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
        else if (key.Key == ConsoleKey.Tab)
        {
          Focus.ChangeFocus();
        }
      }
    }
  }
}