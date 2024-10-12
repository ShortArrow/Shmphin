using System.CommandLine;

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
  public readonly RootCommand rootCommand =
  [
    sharedMemoryNameOption,
    sharedMemorySizeOption,
    sharedMemoryOffsetOption,
    configFileOption
  ];
  public readonly Command testCommand = new("test");
  public readonly Command genCommand = new("gen");
  public Parser()
  {
    rootCommand.AddCommand(testCommand);
    rootCommand.AddCommand(genCommand);
    rootCommand.SetHandler(
        Handler.TUI,
        sharedMemoryNameOption, sharedMemorySizeOption, sharedMemoryOffsetOption, configFileOption
    );
  }
}