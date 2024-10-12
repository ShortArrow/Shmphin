namespace main.config;

public class Config
{
  private static string? sharedMemoryName = Toml.GetSharedMemoryName();
  public static string? SharedMemoryName
  {
    get => sharedMemoryName;
    set => sharedMemoryName = value;
  }
  public static void SyncConfig()
  {
    Toml.SyncToml();
  }
  public static void UpdateConfig(string? path = null)
  {
    Toml.UpdateTomlPath(path);
    Toml.SyncToml();
  }
  public static string? GetSharedMemoryName()
  {
    return Toml.GetSharedMemoryName();
  }
}