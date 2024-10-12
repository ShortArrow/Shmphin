using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Parsing;

namespace main.cli;

public class Parser
{
  static readonly Option<string> sharedMemoryNameOption = new(
    aliases: ["-n", "--name"], description: "Shared memory name"
  );
  static readonly Option<uint?> sharedMemorySizeOption = new(
    aliases: ["-s", "--size"], description: "Shared memory size"
  );
  static readonly Option<uint?> sharedMemoryOffsetOption = new(
    aliases: ["-o", "--offset"], description: "Shared memory offset"
  );
  static readonly Option<string> configFileOption = new(
    aliases: ["-c", "--config"], description: "Config file path"
  );
  private static readonly string splash = "Shmphin";
  public readonly RootCommand rootCommand = new(description: splash)
  {
    sharedMemoryNameOption,
    sharedMemorySizeOption,
    sharedMemoryOffsetOption,
    configFileOption
};
  public readonly Command testCommand = new(name: "test", description: "Test command");
  public readonly Command testConfigCommand = new(name: "config", description: "Test config command");
  public readonly Command dumpCommand = new(name: "dump", description: "Dump Shared Memory");
  public readonly Command helpCommand = new(name: "help", description: "Show help");
  public System.CommandLine.Parsing.Parser rootCommandWithSplash;
  public Parser()
  {
    rootCommand.AddCommand(testCommand);
    rootCommand.AddCommand(dumpCommand);
    rootCommand.AddCommand(helpCommand);
    testCommand.AddCommand(testConfigCommand);

    rootCommand.SetHandler(
        Handler.TUI,
        sharedMemoryNameOption, sharedMemorySizeOption, sharedMemoryOffsetOption, configFileOption
    );
    rootCommandWithSplash = new CommandLineBuilder(rootCommand)
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
    helpCommand.SetHandler(() => rootCommandWithSplash.InvokeAsync(["--help"]));
  }
}