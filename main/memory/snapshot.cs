using System.Runtime.Versioning;

using main.config;

namespace main.memory;

public class SnapShot
{
  private readonly IConfig? config;
  private readonly SharedMemory sharedMemory;
  private byte[] _before = [];
  private byte[] _current = [];
  public SnapShot(IConfig conf)
  {
    config = conf ?? throw new NullReferenceException();
    sharedMemory = new(conf);
    if (config.SharedMemorySize == null) throw new NullReferenceException();
    _before = new byte[(uint)config.SharedMemorySize];
    _current = new byte[(uint)config.SharedMemorySize];
  }

  [SupportedOSPlatform("windows")]
  public void Update()
  {
    _before = _current;
    if (config == null) throw new NullReferenceException();
    if (config.SharedMemoryOffset == null) throw new NullReferenceException();
    if (config.SharedMemorySize == null) throw new NullReferenceException();
    _current = sharedMemory.ReadFromSharedMemory((int)config.SharedMemoryOffset, (int)config.SharedMemorySize);
  }
  public byte[] Before
  {
    get => _before;
    set => _before = value;
  }
  public byte[] Current
  {
    get => _current;
    set => _current = value;
  }
}
