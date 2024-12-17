using main.ui;
using main.ui.keyhandler;

namespace main.operation;
class Help(Mode mode) : Operation()
{
  public override string Name => "help";
  public override void Execute()
  {
    mode.InputMode = InputMode.Help;
  }
}
