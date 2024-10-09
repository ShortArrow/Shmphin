
using System.Text;
using Spectre.Console;

namespace main.ui;

class Input
{
  private static StringBuilder inputBuffer = new StringBuilder();
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
      if (key.KeyChar.ToString()
      .Equals("q", StringComparison.CurrentCultureIgnoreCase))
      {
        cts.Cancel(); // Request cancellation
      }
      else if (key.KeyChar.ToString().Equals("u", StringComparison.CurrentCultureIgnoreCase))
      {
        uts.TrySetResult(true);
      }
    }
  }
}