using main.config;
using main.ui;

namespace main.operation;
class SharedMemoryName(ICurrentConfig config, IMode mode) : IOperation
{
  public string Name => "SharedMemoryName";
  public async Task Execute()
  {
    var sharedMemoryName = await mode.NewSharedMemoryName();
    config.SharedMemoryName = sharedMemoryName;
  }
}
