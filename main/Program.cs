using Microsoft.Extensions.DependencyInjection;

namespace main;

class Program
{
  static async Task<int> Main(string[] args)
  {
    var services = new ServiceCollection();

    services.AddSingleton<IConsole>(implementationFactory: static _ => new DefaultConsole
    {
      IsEnabled = true
    });
    services.AddSingleton<IApp, App>();

    var serviceProvider = services.BuildServiceProvider();

    var app = serviceProvider.GetRequiredService<IApp>();

    var result =  await app.MainProcess(args);

    return result;
  }
}
