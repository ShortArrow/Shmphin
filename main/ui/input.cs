
using System.Text;
using Spectre.Console;

namespace main.ui;

class Input
{
  public static StringBuilder inputBuffer = new();
  public static string GetSharedMemoryName()
  {
    return AnsiConsole.Prompt(
      new TextPrompt<string>("Enter the shared memory name:")
    );
  }
  public static void InputLoop(CancellationTokenSource cts, TaskCompletionSource<bool> uts)
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
        if (name.Equals("q", StringComparison.CurrentCultureIgnoreCase))
        {
          cts.Cancel(); // Request cancellation
        }
        else if (name.Equals("u", StringComparison.CurrentCultureIgnoreCase))
        {
          uts.TrySetResult(true); // Request update SnapShot
        }
        else if (name.Equals(":", StringComparison.CurrentCultureIgnoreCase))
        {
          Ui.IsExMode = true;  // Enter Ex mode
          inputBuffer.Clear();
          inputBuffer.Append('>');
        }
      }
    }
  }
}