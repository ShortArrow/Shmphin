using System.CommandLine;

namespace main.cli;

class Options
{
  internal static readonly Option<string> sharedMemoryName = new(
    aliases: ["-n", "--name"], description: "Shared memory name"
  );
  internal static readonly Option<uint?> sharedMemorySize = new(
    aliases: ["-s", "--size"], description: "Shared memory size"
  );
  internal static readonly Option<uint?> sharedMemoryOffset = new(
    aliases: ["-o", "--offset"], description: "Shared memory offset"
  );
  internal static readonly Option<string> configFile = new(
    aliases: ["-c", "--config"], description: "Config file path"
  );
}