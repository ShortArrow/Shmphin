using main.ui;

namespace main.operation;

class Columns(Input input) : Operation()
{
  public override string Name => "columns";
  public override async void Execute()
  {
    var columnsLength = await input.NewColumnsLength();
    GridMode.ColumnsLength = columnsLength;
  }
}
