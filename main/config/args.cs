namespace main.config;

public interface IArgs : IConfig
{
}
public class Args(
  string? name,
  uint? cellLength,
  uint? columnsLength,
  uint? sharedMemorySize,
  uint? sharedMemoryOffset
) : IArgs
{
  public string? SharedMemoryName => name;
  public uint? ColumnsLength => columnsLength;
  public uint? CellLength => cellLength;
  public uint? SharedMemorySize => sharedMemorySize;
  public uint? SharedMemoryOffset => sharedMemoryOffset;
}
