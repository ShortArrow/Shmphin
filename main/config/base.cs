namespace main.config;

class Common
{
  public static readonly string AppName = "Shmphin";
}

public interface IConfigFile
{
  abstract string? GetSharedMemoryName();
  abstract uint? GetColumnsLength();
  abstract uint? GetCellLength();
  abstract uint? GetSharedMemorySize();
  abstract uint? GetSharedMemoryOffset();
}

public interface IConfig
{
  string? SharedMemoryName { get; }
  uint? ColumnsLength { get; }
  uint? CellLength { get; }
  uint? SharedMemorySize { get; }
  uint? SharedMemoryOffset { get; }
}
