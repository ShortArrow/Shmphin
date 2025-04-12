using main.config;
using main.ui;

namespace main.operation;

class Columns(ICurrentConfig config, IMode mode) : Operation()
{
  public override string Name => "columns";
  public override async void Execute()
  {
    var columnsLength = await mode.NewColumnsLength();
    config.ColumnsLength = columnsLength;
  }
}
