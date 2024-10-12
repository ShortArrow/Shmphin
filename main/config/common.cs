namespace main.config;

class Common
{
  public static readonly string AppName = "Shmphin";
}

public interface IConfigFile
{
  public static abstract string? GetSharedMemoryName();
}