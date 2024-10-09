
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
      if (name.Equals("q", StringComparison.CurrentCultureIgnoreCase))
      {
        cts.Cancel(); // Request cancellation
      }
      else if (name.Equals("u", StringComparison.CurrentCultureIgnoreCase))
      {
        uts.TrySetResult(true);
      }
      else if (name.Equals(":", StringComparison.CurrentCultureIgnoreCase))
      {
        Ui.IsExMode = true;
        inputBuffer.Clear();
        inputBuffer.Append(">");
      }
      else if (Ui.IsExMode)
      {
        if(name.Equals("{Enter}", StringComparison.CurrentCultureIgnoreCase))
        {
          Ui.IsExMode = false;
          // Execute the command
          inputBuffer.Clear();
          inputBuffer.Append("Command executed.");
        }
        else if(name.Equals("{Backspace}", StringComparison.CurrentCultureIgnoreCase))
        {
          inputBuffer.Remove(inputBuffer.Length - 1, 1);
        }
        else
        {
          inputBuffer.Append(key.KeyChar);
        }
      }
    }
  }
}