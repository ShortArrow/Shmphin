namespace main.config;

class Common
{
  public static readonly string AppName = "Shmphin";
}

public interface IConfigFile
{
  public abstract string? GetSharedMemoryName();
  public abstract uint? GetColumnsLength();
  public abstract uint? GetCellLength();
  public abstract uint? GetSharedMemorySize();
  public abstract uint? GetSharedMemoryOffset();
}

public interface IConfig
{
  public abstract string? SharedMemoryName { get; }
  public abstract uint? ColumnsLength { get; }
  public abstract uint? CellLength { get; }
  public abstract uint? SharedMemorySize { get; }
  public abstract uint? SharedMemoryOffset { get; }
}
