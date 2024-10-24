using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Parsing;

namespace main.cli;

public class Parser
{
  public System.CommandLine.Parsing.Parser rootCommandWithSplash;
  public Parser()
  {
    var rootHandler = new handler.Root();
    var testHandler = new handler.Test();
    Commands.rootCommand.AddCommand(Commands.testCommand);
    Commands.rootCommand.AddCommand(Commands.dumpCommand);
    Commands.rootCommand.AddCommand(Commands.helpCommand);
    Commands.testCommand.AddCommand(Commands.testConfigCommand);

    Commands.rootCommand.SetHandler(rootHandler.InvokeAsync);
    Commands.testCommand.SetHandler(testHandler.InvokeAsync);
    rootCommandWithSplash = new CommandLineBuilder(Commands.rootCommand)
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
    Commands.helpCommand.SetHandler(() => rootCommandWithSplash.InvokeAsync(["--help"]));
  }
}
