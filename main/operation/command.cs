using main.ui;
using main.ui.keyhandler;

namespace main.operation;
public class ExCommand(IMode mode) : IOperation
{
  public string Name => "command";
  public Task Execute()
  {
    mode.InputMode = InputMode.Ex;
    return Task.CompletedTask;
    // clear the input buffer
    // input buffer starts with '>'
  }
}
