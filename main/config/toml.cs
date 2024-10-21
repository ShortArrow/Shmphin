using Tommy;

namespace main.config;

class Toml : IConfigFile
{
  private static readonly string defaultConfigName = "config.toml";
  private string configPath = defaultConfigName;
  private TomlTable? table;
  public void UpdateTomlPath(string? path = null)
  {
    if (path != null)
    {
      if (!Path.Exists(path)) throw new FileNotFoundException($"File not found: {path}");
      configPath = path;
    }
    else
    {
      string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
      configPath = Path.Combine(appDataPath, Common.AppName, defaultConfigName);
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
  public TomlTable GetToml()
  {
    using StreamReader reader = File.OpenText(configPath);
    return TOML.Parse(reader);
  }
  public void SyncToml()
  {
    table = GetToml();
  }
  public void GenToml()
  {
    throw new NotImplementedException();
  }
  public string? GetSharedMemoryName()
  {
    return table?["default"]["sharedmemory"]["name"].ToString();
  }
  public uint? GetColumnsLength()
  {
    var response = table?["default"]["columns"]["length"]?.ToString();
    return response != null ? Convert.ToUInt32(response) : null;
  }
  public uint? GetCellLength()
  {
    var response = table?["default"]["cell"]["length"]?.ToString();
    return response != null ? Convert.ToUInt32(response) : null;
  }
  public static string GenerateToml()
  {
    var toml = new TomlTable
    {
      ["default"] = new TomlTable
      {
        ["sharedmemory"] = new TomlTable
        {
          ["name"] = "shmphin"
        },
        ["columns"] = new TomlTable
        {
          ["length"] = 8
        },
        ["cell"] = new TomlTable
        {
          ["length"] = 1
        }
      }
    };
    using var writer = new StringWriter();
    toml.WriteTo(writer);
    return writer.ToString();
  }
}
