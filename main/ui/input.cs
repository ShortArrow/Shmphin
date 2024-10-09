
using Spectre.Console;

namespace main.ui;

class Input
{
  public static string GetSharedMemoryName()
  {
    return AnsiConsole.Prompt(
      new TextPrompt<string>("Enter the shared memory name:")
    );
  }
}