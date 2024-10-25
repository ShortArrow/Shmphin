using main.config;
using main.operation;

namespace main.memory;

public class Memory(IConfig config) : Operation()
{
  private readonly SharedMemory sharedMemory = new(config);
  private readonly SnapShot snapShot = new(config);
  public SharedMemory SharedMemory => sharedMemory;
  public SnapShot SnapShot => snapShot;
}
