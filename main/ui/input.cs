
using Spectre.Console;

namespace main.ui;

class Input
{
  public static string GetSharedMemoryName()
  {
    AnsiConsole.MarkupLine($"Welcome! [bold green]Shmphin[/] is a shared memory editor.");
    return AnsiConsole.Prompt(
      new TextPrompt<string>("Enter the shared memory name:")
    );
  }
}