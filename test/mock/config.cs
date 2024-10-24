using main.config;

namespace test.mock;

public class MockConfig(
  string? sharedMemoryName,
  uint? cellLength,
  uint? columnsLength,
  uint? sharedMemorySize,
  uint? sharedMemoryOffset

) : IConfig
{
  public string? SharedMemoryName => sharedMemoryName;
  public uint? CellLength => cellLength;
  public uint? ColumnsLength => columnsLength;
  public uint? SharedMemorySize => sharedMemorySize;
  public uint? SharedMemoryOffset => sharedMemoryOffset;
}
