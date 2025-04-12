using Microsoft.Extensions.DependencyInjection;

namespace main.di;

/// <summary>
/// This class sets up the Dependency Injection container.
/// </summary>
public static class Container
{
  /// <summary>
  /// Configures services for the application.
  /// </summary>
  /// <param name="services">The IServiceCollection instance.</param>
  /// <returns>A built IServiceProvider.</returns>
  public static IServiceProvider ConfigureServices(IServiceCollection services)
  {
    // Register DefaultConsole as IConsole
    services.AddSingleton<IConsole, DefaultConsole>();

    // Register App as IApp
    services.AddSingleton<IApp, App>();

    // Additional service registrations can be added here

    return services.BuildServiceProvider();
  }
}
