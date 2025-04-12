
using main.config;

using Spectre.Console;

namespace main.ui;

public interface IQuestion
{
  Task GetSharedMemoryName();
}

public class Question(ICurrentConfig config) : IQuestion
{
  public Task GetSharedMemoryName()
  {
    config.SharedMemoryName = AnsiConsole.Prompt(
      new TextPrompt<string>("Enter the shared memory name:")
    );
    return Task.CompletedTask;
  }
}
