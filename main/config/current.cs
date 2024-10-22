namespace main.config;

public class CurrentConfig(Command command) : IConfig
{
  private readonly TomlConfig toml = new();
  private readonly Default defaultConf = new();
  private string? sharedMemoryName;
  public string? SharedMemoryName
  {
    get =>
      sharedMemoryName
      ?? command.SharedMemoryName
      ?? toml.SharedMemoryName
      ?? defaultConf.SharedMemoryName;
    set => sharedMemoryName = value;
  }
  private uint? columnsLength;
  public uint? ColumnsLength
  {
    get =>
      columnsLength
      ?? command.ColumnsLength
      ?? toml.ColumnsLength
      ?? defaultConf.ColumnsLength;
    set => columnsLength = value;
  }
  private uint? cellLength;
  public uint? CellLength
  {
    get =>
      cellLength
      ?? command.CellLength
      ?? toml.CellLength
      ?? defaultConf.CellLength;
    set => cellLength = value;
  }
  private uint? sharedMemorySize;
  public uint? SharedMemorySize
  {
    get =>
      sharedMemorySize
      ?? command.SharedMemorySize
      ?? toml.SharedMemorySize
      ?? defaultConf.SharedMemorySize;
    set => sharedMemorySize = value;
  }
  private uint? sharedMemoryOffset;
  public uint? SharedMemoryOffset
  {
    get =>
      sharedMemoryOffset
      ?? command.SharedMemoryOffset
      ?? toml.SharedMemoryOffset
      ?? defaultConf.SharedMemoryOffset;
    set => sharedMemoryOffset = value;
  }
  public void Sync()
  {
    toml.Sync();
  }
  public void UpdateConfig(string? configFile)
  {
    if (configFile != null)
    {
      toml.UpdateConfig(configFile);
    }
  }
}
