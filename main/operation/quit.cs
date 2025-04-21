using main.ui;

namespace main.operation;
class Quit(IMode mode) : IOperation
{
  public string Name => "quit";
  public Task Execute()
  {
    mode.Cts.Cancel();
    return Task.CompletedTask;
  }
}
