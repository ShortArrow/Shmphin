using Microsoft.Extensions.DependencyInjection;
using main.config;
using main.cli.handler;
using main.cli;
using main.model;
using main.memory;
using main.ui;
using main.operation;
using main.ui.keyhandler;
using main.ui.layout;

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

    // Register CurrentConfig as ICurrentConfig using a factory for initial default values.
    // The command object is created with default (null) values and will be updated later.
    services.AddSingleton<ICurrentConfig>(provider =>
    {
      var defaultCommand = new Args(null, null, null, null, null);
      return new CurrentConfig(defaultCommand);
    });

    services.AddSingleton<IParser, cli.Parser>();

    // Register cli handlers
    services.AddSingleton<IRoot, Root>();
    services.AddSingleton<ITest, Test>();

    services.AddSingleton<IQuestion, Question>();
    services.AddSingleton<ICursor, model.Cursor>();
    services.AddSingleton<IMemory, Memory>();
    services.AddSingleton<IMode, Mode>();
    services.AddSingleton<IFocus, Focus>();
    services.AddSingleton<ISnapShot, SnapShot>();
    // services.AddSingleton<IOperation, Operation>();
    services.AddSingleton<IOperations, Operations>();
    services.AddSingleton(provider => (Operations)provider.GetRequiredService<IOperations>());
    services.AddSingleton<IUi, Ui>();
    services.AddSingleton<IInput>(provider =>
    {
      var operations = provider.GetRequiredService<Operations>();
      var mode = provider.GetRequiredService<IMode>();
      var lazySelectView = new Lazy<ISelectView>(() => provider.GetRequiredService<ISelectView>());
      return new Input(operations, mode);
    });
    services.AddSingleton<ISelectView, SelectView>();

    // Register App as IApp
    services.AddSingleton<IApp, App>();

    // Additional service registrations can be added here

    return services.BuildServiceProvider();
  }
}
