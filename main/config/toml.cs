using Tommy;

namespace main.config;

class Toml : IConfigFile
{
  private static readonly string configName = "config.toml";
  private static string configPath = configName;
  private static TomlTable? table;
  public static void UpdateTomlPath(string? path = null)
  {
    if (path != null)
    {
      if (!Path.Exists(path)) throw new FileNotFoundException($"File not found: {path}");
      configPath = path;
    }
    else
    {
      string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
      configPath = Path.Combine(appDataPath, Common.AppName, configName);
      if (!Directory.Exists(appDataPath))
      {
        Directory.CreateDirectory(appDataPath);
      }
      if (!Path.Exists(configPath))
      {
        File.Create(configPath).Close();
      }
    }
  }
  public static TomlTable GetToml()
  {
    using StreamReader reader = File.OpenText(configPath);
    return TOML.Parse(reader);
  }
  public static void SyncToml()
  {
    table = GetToml();
  }
  public static void GenToml()
  {

  }
  public static string? GetSharedMemoryName()
  {
    return table?["default"]["sharedmemory"]["name"].ToString();
  }
}
