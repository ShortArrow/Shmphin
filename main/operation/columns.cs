using main.config;
using main.ui;

namespace main.operation;

class Columns(ICurrentConfig config, IMode mode) : IOperation
{
  public string Name => "columns";
  public async Task Execute()
  {
    var columnsLength = await mode.NewColumnsLength();
    config.ColumnsLength = columnsLength;
  }
}
