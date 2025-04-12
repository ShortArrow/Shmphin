using System.CommandLine;

namespace main.cli;

class Commands
{
  internal static readonly RootCommand rootCommand = new(description: "Shmphin is a shared memory editor")
  {
    Options.sharedMemoryName,
    Options.sharedMemorySize,
    Options.sharedMemoryOffset,
    Options.cellSize,
    Options.columnsLength,
    Options.configFile
};
  internal static readonly Command testCommand = new(name: "test", description: "Test command")
  {
    Options.sharedMemoryName,
    Options.sharedMemorySize,
    Options.sharedMemoryOffset,
    Options.cellSize,
    Options.columnsLength,
    Options.configFile
  };
  internal static readonly Command testConfigCommand = new(name: "config", description: "Test config command");
  internal static readonly Command dumpCommand = new(name: "dump", description: "Dump Shared Memory");
  internal static readonly Command helpCommand = new(name: "help", description: "Show help");

}
