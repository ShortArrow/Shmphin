using System.Runtime.Versioning;
namespace main.memory;

class SnapShot
{
  private static byte[] _before = new byte[8];
  private static byte[] _current = new byte[8];

  [SupportedOSPlatform("windows")]
  public static void UpdateSnapShot()
  {
    _before = _current;
    _current = SharedMemoryHelper.ReadFromSharedMemory(Params.Offset, 24);
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