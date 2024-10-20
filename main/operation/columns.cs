using main.ui;

namespace main.operation;

class Columns : Operation
{
  public override string Name => "columns";
  public override async void Execute()
  {
    var columnsLength = await Input.NewColumnsLength();
    GridMode.ColumnsLength = columnsLength;
  }
}
