using main.config;
using main.ui;

namespace main.operation;
class Cell(ICurrentConfig config, IMode mode) : IOperation
{
  public string Name => "cell";
  public async Task Execute()
  {
    var cellsize = await mode.NewCellSize();
    config.CellLength = cellsize;
  }
}
