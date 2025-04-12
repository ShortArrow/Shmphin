using main.config;
using main.ui;

namespace main.operation;
class SharedMemoryName(ICurrentConfig config, IMode mode) : Operation()
{
  public override string Name => "SharedMemoryName";
  public override async void Execute()
  {
    var sharedMemoryName = await mode.NewSharedMemoryName();
    config.SharedMemoryName = sharedMemoryName;
  }
}
