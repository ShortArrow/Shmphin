namespace main.config;

public interface IDefaultConfig : IConfig
{
}
class Default : IDefaultConfig
{
  public string? SharedMemoryName => null;
  public uint? ColumnsLength => 8;
  public uint? CellLength => 1;
  public uint? SharedMemorySize => 512;
  public uint? SharedMemoryOffset => 0;
}
