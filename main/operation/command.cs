using main.ui;
using main.ui.keyhandler;

namespace main.operation;
public class ExCommand(IMode mode) : Operation
{
  public override string Name => "command";
  public override void Execute()
  {
    mode.InputMode = InputMode.Ex;
    // clear the input buffer
    // input buffer starts with '>'
  }
}
