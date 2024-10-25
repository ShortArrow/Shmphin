using main.config;
using main.ui;

namespace main.operation;
class Cell(CurrentConfig config, Mode mode) : Operation()
{
  public override string Name => "cell";
  public override async void Execute()
  {
    var cellsize = await mode.NewCellSize();
    config.ColumnsLength = cellsize;
  }
}
