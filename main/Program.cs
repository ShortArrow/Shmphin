using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace main;

class Program
{
  static async Task<int> Main(string[] args)
  {
    HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
    builder.Services.AddSingleton<IConsole, DefaultConsole>();
    var services = new ServiceCollection();
    using IHost host = builder.Build();

    services.AddSingleton<IConsole>(implementationFactory: static _ => new DefaultConsole
    {
      IsEnabled = true
    });
    services.AddSingleton<IApp, App>();

    var serviceProvider = services.BuildServiceProvider();

    var app = serviceProvider.GetRequiredService<IApp>();

    var result = await app.MainProcess(args);

    return result;
  }
}
