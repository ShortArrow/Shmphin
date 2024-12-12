using main.ui;

namespace main.operation;
class Help(Mode mode) : Operation()
{
  public override string Name => "help";
  public override void Execute()
  {
    mode.IsKeymapShown = true;
  }
}
