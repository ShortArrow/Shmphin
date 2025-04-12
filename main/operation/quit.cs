using main.ui;

namespace main.operation;
class Quit(IMode mode) : Operation()
{
  public override string Name => "quit";
  public override void Execute()
  {
    mode.cts.Cancel();
  }
}
