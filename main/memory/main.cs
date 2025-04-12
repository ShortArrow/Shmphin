using main.config;

namespace main.memory;

public interface IMemory
{
  SharedMemory SharedMemory { get; }
}

public class Memory(ICurrentConfig config) : IMemory
{
  private readonly SharedMemory sharedMemory = new(config);
  public SharedMemory SharedMemory => sharedMemory;
}
