namespace main.config;
public class Command(string? name, uint? cellLength, uint? columnsLength) : IConfig
{
  private readonly string? sharedmemoryName = name;
  private readonly uint? cellLength = cellLength;
  private readonly uint? columnsLength = columnsLength;
  public string? SharedMemoryName => sharedmemoryName;
  public uint? ColumnsLength => columnsLength;
  public uint? CellLength => cellLength;
}
