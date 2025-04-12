using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Parsing;

using main.cli.handler;

namespace main.cli;

public interface IParser
{
  System.CommandLine.Parsing.Parser RootCommandWithSplash { get; }
}

public class Parser : IParser
{
  public System.CommandLine.Parsing.Parser RootCommandWithSplash { get; private set; }
  public Parser(IRoot rootHandler, ITest testHandler)
  {
    Commands.rootCommand.AddCommand(Commands.testCommand);
    Commands.rootCommand.AddCommand(Commands.dumpCommand);
    Commands.rootCommand.AddCommand(Commands.helpCommand);
    Commands.testCommand.AddCommand(Commands.testConfigCommand);

    Commands.rootCommand.SetHandler(rootHandler.InvokeAsync);
    Commands.testCommand.SetHandler(testHandler.InvokeAsync);
    RootCommandWithSplash = new CommandLineBuilder(Commands.rootCommand)
      .UseDefaults()
      .UseHelp(context =>
      {
        context.HelpBuilder.CustomizeLayout(
          _ => HelpBuilder.Default
            .GetLayout()
            .Skip(1)
            .Prepend(
              _ =>
              {
                ui.Welcome.Show();
              }
            )
        );
      })
      .Build() ?? throw new Exception("Command build failed");
    Commands.helpCommand.SetHandler(() => RootCommandWithSplash.InvokeAsync(["--help"]));
  }
}
