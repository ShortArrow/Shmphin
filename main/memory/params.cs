
namespace main.memory;

class Params
{
  private static string _sharedMemoryName = string.Empty;
  private static uint _size = 24;
  private static uint _offset = 0;
  public static string SharedMemoryName
  {
    get => _sharedMemoryName;
    set => _sharedMemoryName = value;
  }
  public static uint Size
  {
    get => _size;
    set => _size = value;
  }
  public static uint Offset
  {
    get => _offset;
    set => _offset = value;
  }
}