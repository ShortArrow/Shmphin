using Microsoft.Extensions.Logging;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;

namespace main.logging;

/// <summary>
/// Provides methods for configuring NLog logging.
/// </summary>
public static class NLogConfigurator
{
  // Cache the generated log file instance to ensure a consistent file path.
  private static LogFile? _logFileCache;

  /// <summary>
  /// Returns the generated log filename based on configuration (cached).
  /// </summary>
  /// <param name="configService">The configuration service used to obtain settings.</param>
  /// <returns>The generated log filename.</returns>
  public static LogFile GetLogFile()
  {
    if (_logFileCache != null)
    {
      return _logFileCache;
    }
    var logFolder = "./";
    Directory.CreateDirectory(logFolder);
    var filename = $@"main_{DateTime.Now:yyyyMMdd_HHmmss}.log";
    var fullPath = Path.Combine(logFolder, filename);
    _logFileCache = new LogFile(fullPath, logFolder, filename);
    return _logFileCache;
  }

  public static NLog.LogLevel ConvertLogLevel(LogLevel logLevel)
  {
    return logLevel switch
    {
      LogLevel.Trace => NLog.LogLevel.Trace,
      LogLevel.Debug => NLog.LogLevel.Debug,
      LogLevel.Information => NLog.LogLevel.Info,
      LogLevel.Warning => NLog.LogLevel.Warn,
      LogLevel.Error => NLog.LogLevel.Error,
      LogLevel.Critical => NLog.LogLevel.Fatal,
      LogLevel.None => NLog.LogLevel.Off,
      _ => NLog.LogLevel.Info,
    };
  }

  /// <summary>
  /// Configures NLog using the provided logging builder, configuration service, and log filename.
  /// </summary>
  /// <param name="loggingBuilder">The logging builder to configure.</param>
  /// <param name="logFilename">The log filename to use.</param>
  /// <param name="logLevelForFile">The log level for the file target.</param>
  /// <param name="logLevelForConsole">The log level for the console target.</param>
  public static void ConfigureLogging(ILoggingBuilder loggingBuilder, string logFilename, LogLevel logLevelForFile, LogLevel logLevelForConsole)
  {
    // Set up NLog configuration.
    var nlogConfiguration = new LoggingConfiguration();
    var fileTarget = new FileTarget("logfile")
    {
      FileName = logFilename,
    };
    var consoleTarget = new ConsoleTarget("logconsole");
    consoleTarget.Encoding = System.Text.Encoding.UTF8;
    consoleTarget.Layout = "${longdate} | ${level:uppercase=true:padding=-5} | ${logger:padding=-50} | ${message} ${exception:format=toString,StackTrace}";
    var fileLoglevel = ConvertLogLevel(logLevelForFile);
    var consoleLoglevelForConsole = ConvertLogLevel(logLevelForConsole);

    // Configure log rules (Trace to Fatal) in NLog.
    nlogConfiguration.AddRule(consoleLoglevelForConsole, NLog.LogLevel.Fatal, consoleTarget);
    nlogConfiguration.AddRule(fileLoglevel, NLog.LogLevel.Fatal, fileTarget);

    NLog.LogManager.ThrowConfigExceptions = true;
    NLog.LogManager.Configuration = nlogConfiguration;

    // Clear default providers and add NLog provider.
    loggingBuilder.ClearProviders();
    loggingBuilder.AddNLog();
  }
}
