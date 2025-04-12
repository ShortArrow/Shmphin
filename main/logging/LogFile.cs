namespace main.logging;

/// <summary>
/// Represents a log file.
/// </summary>
public interface ILogFile
{
  string FullPath { get; }
  string Directory { get; }
  string Name { get; }
}

/// <summary>
/// Represents a log file.
/// </summary>
public sealed class LogFile(string fullpath, string directory, string filename) : ILogFile
{
  public string FullPath { get; } = fullpath;
  public string Directory { get; } = directory;
  public string Name { get; } = filename;
}
