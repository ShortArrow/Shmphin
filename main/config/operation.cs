namespace main.config;

public class CurrentConfig
{
  private readonly Toml toml = new();
  private string? sharedMemoryName;
  public string? SharedMemoryName
  {
    get => sharedMemoryName;
    set => sharedMemoryName = value;
  }
  public CurrentConfig()
  {
    sharedMemoryName = toml.GetSharedMemoryName();
  }
  public void SyncConfig()
  {
    toml.SyncToml();
  }
  public void UpdateConfig(string? path = null)
  {
    toml.UpdateTomlPath(path);
    toml.SyncToml();
  }
  public string? GetSharedMemoryName()
  {
    return toml.GetSharedMemoryName();
  }
}
