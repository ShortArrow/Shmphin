namespace main.config;

public class Config(
  string? name,
  uint? cellLength,
  uint? columnsLength,
  uint? sharedMemorySize,
  uint? sharedMemoryOffset
) : IConfig
{
  private readonly string? sharedmemoryName = name;
  private readonly uint? cellLength = cellLength;
  private readonly uint? columnsLength = columnsLength;
  public string? SharedMemoryName => sharedmemoryName;
  public uint? ColumnsLength => columnsLength;
  public uint? CellLength => cellLength;
  public uint? SharedMemorySize => sharedMemorySize;
  public uint? SharedMemoryOffset => sharedMemoryOffset;
}
