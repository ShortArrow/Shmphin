using main.memory;

namespace main.config;

public class CurrentConfig(Command command) : IConfig
{
  private readonly TomlConfig toml = new();
  private string? sharedMemoryName;
  public string? SharedMemoryName
  {
    get =>
      sharedMemoryName
      ?? command.SharedMemoryName
      ?? toml.SharedMemoryName
      ?? Params.SharedMemoryName;
    set => sharedMemoryName = value;
  }
  private uint? columnsLength;
  public uint? ColumnsLength
  {
    get =>
      columnsLength
      ?? command.ColumnsLength
      ?? toml.ColumnsLength;
    set => columnsLength = value;
  }
  private uint? cellLength;
  public uint? CellLength
  {
    get =>
      cellLength
      ?? command.CellLength
      ?? toml.CellLength;
    set => cellLength = value;
  }
  public void Sync()
  {
    toml.Sync();
  }
  public void UpdateConfig(string configFile)
  {
    toml.UpdateConfig(configFile);
  }
}
