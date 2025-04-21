using main.ui;
using main.ui.keyhandler;

namespace main.operation;
class Help(IMode mode) : IOperation
{
  public string Name => "help";
  public Task Execute()
  {
    mode.InputMode = InputMode.Help;
    return Task.CompletedTask;
  }
}
