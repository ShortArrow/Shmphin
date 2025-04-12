using Microsoft.Extensions.DependencyInjection;
using main.di;

namespace main;

/// <summary>
/// Main entry point of the application.
/// </summary>
class Program
{
  /// <summary>
  /// Application entry point.
  /// </summary>
  /// <param name="args">Command-line arguments.</param>
  /// <returns>Application exit code.</returns>
  static async Task<int> Main(string[] args)
  {
    // Create a new ServiceCollection
    var services = new ServiceCollection();

    // Configure services using the DI container configuration
    var serviceProvider = Container.ConfigureServices(services);

    // Retrieve the application instance
    var app = serviceProvider.GetRequiredService<IApp>();

    // Execute the main process and return the result
    return await app.MainProcess(args);
  }
}
