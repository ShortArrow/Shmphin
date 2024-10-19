using main.ui;

namespace main.operation;
class Cell : Operation
{
  public override string Name => "cell";
  public override async void Execute()
  {
    var cellsize = await Input.NewCellSize();
    GridMode.CellLength = cellsize;
  }
}
