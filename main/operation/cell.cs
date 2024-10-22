using main.config;
using main.ui;

namespace main.operation;
class Cell(Input input) : Operation()
{
  public override string Name => "cell";
  public override async void Execute()
  {
    var cellsize = await input.NewCellSize();
    GridMode.CellLength = cellsize;
  }
}
