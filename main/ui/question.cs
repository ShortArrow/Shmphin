
using main.config;

using Spectre.Console;

namespace main.ui;

public class Question(CurrentConfig config)
{
  public Task GetSharedMemoryName()
  {
    config.SharedMemoryName = AnsiConsole.Prompt(
      new TextPrompt<string>("Enter the shared memory name:")
    );
    return Task.CompletedTask;
  }
}
