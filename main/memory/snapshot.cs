using System.Runtime.Versioning;
namespace main.memory;

class SnapShot
{
  private static byte[] _before = new byte[Params.Size];
  private static byte[] _current = new byte[Params.Size];

  [SupportedOSPlatform("windows")]
  public static void UpdateSnapShot()
  {
    _before = _current;
    _current = SharedMemory.ReadFromSharedMemory(Params.Offset, (int)Params.Size);
  }
  public static byte[] Before
  {
    get => _before;
    set => _before = value;
  }
  public static byte[] Current
  {
    get => _current;
    set => _current = value;
  }
}