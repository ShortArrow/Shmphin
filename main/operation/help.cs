using main.ui;
using main.ui.keyhandler;

namespace main.operation;
class Help(IMode mode) : Operation()
{
  public override string Name => "help";
  public override void Execute()
  {
    mode.InputMode = InputMode.Help;
  }
}
