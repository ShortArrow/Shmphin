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
  public string? GetSharedMemoryName()
  {
    var token = table?["default"]["sharedmemory"]["name"];
    return (string?)(token ?? token?.IsString ? token?.AsString : null);
  }
  public uint? GetColumnsLength()
  {
    var token = table?["default"]["columns"]["length"];
    return (uint?)(token ?? token?.IsInteger ? token?.AsInteger : null);
  }
  public uint? GetCellLength()
  {
    var token = table?["default"]["cell"]["length"];
    return (uint?)(token ?? token?.IsInteger ? token?.AsInteger : null);
  }
  public uint? GetSharedMemorySize()
  {
    var token = table?["default"]["sharedmemory"]["size"];
    return (uint?)(token ?? token?.IsInteger ? token?.AsInteger : null);
  }
  public uint? GetSharedMemoryOffset()
  {
    var token = table?["default"]["sharedmemory"]["offset"];
    return (uint?)(token ?? token?.IsInteger ? token?.AsInteger : null);
  }
  public static string GenerateToml()
  {
    var defaultConfig = new Default();
    var toml = new TomlTable
    {
      ["default"] = new TomlTable
      {
        ["sharedmemory"] = new TomlTable
        {
          ["name"] = defaultConfig.SharedMemoryName
        },
        ["columns"] = new TomlTable
        {
          ["length"] = defaultConfig.ColumnsLength
        },
        ["cell"] = new TomlTable
        {
          ["length"] = defaultConfig.CellLength
        }
      }
    };
    using var writer = new StringWriter();
    toml.WriteTo(writer);
    return writer.ToString();
  }
}

public interface IToml : IConfig
{
}

public class TomlConfig : IToml
{
  private readonly Toml toml = new();
  private string? sharedMemoryName;
  public string? SharedMemoryName => sharedMemoryName;
  private uint? columnsLength;
  public uint? ColumnsLength => columnsLength;
  private uint? cellLength;
  public uint? CellLength => cellLength;
  private uint? sharedMemorySize;
  public uint? SharedMemorySize => sharedMemorySize;
  private uint? sharedMemoryOffset;
  public uint? SharedMemoryOffset => sharedMemoryOffset;
  public void Sync()
  {
    toml.SyncToml();
    sharedMemoryName = toml.GetSharedMemoryName();
    columnsLength = toml.GetColumnsLength();
    cellLength = toml.GetCellLength();
    sharedMemorySize = toml.GetSharedMemorySize();
    sharedMemoryOffset = toml.GetSharedMemoryOffset();
  }
  public void UpdateConfig(string configFile)
  {
    toml.UpdateTomlPath(configFile);
    Sync();
  }
}
