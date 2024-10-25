using main.ui;

namespace main.operation;
public class ExCommand(Mode mode) : Operation
{
  public override string Name => "command";
  public override void Execute()
  {
    mode.InputMode = InputMode.Ex;
    // clear the input buffer
    // input buffer starts with '>'
  }
}
