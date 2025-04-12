using main.config;

namespace test.mock;

public class MockConfig(
  string? sharedMemoryName,
  uint? cellLength,
  uint? columnsLength,
  uint? sharedMemorySize,
  uint? sharedMemoryOffset

) : ICurrentConfig
{
  public string? SharedMemoryName { get; set; } = sharedMemoryName;
  public uint? CellLength { get; set; } = cellLength;
  public uint? ColumnsLength { get; set; } = columnsLength;
  public uint? SharedMemorySize { get; set; } = sharedMemorySize;
  public uint? SharedMemoryOffset { get; set; } = sharedMemoryOffset;
  public void Sync() { }
  public void UpdateConfig(string? configFile) { }
}
