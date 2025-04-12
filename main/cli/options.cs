using System.CommandLine;

namespace main.cli;

class Options
{
  internal static readonly Option<string> sharedMemoryName = new(
    aliases: ["-n", "--name"], description: "Shared memory name"
  );
  internal static readonly Option<uint?> sharedMemorySize = new(
    aliases: ["-S", "--size"], description: "Shared memory size (in bytes)"
  );
  internal static readonly Option<uint?> sharedMemoryOffset = new(
    aliases: ["-o", "--offset"], description: "Shared memory offset (in bytes)"
  );
  internal static readonly Option<string> configFile = new(
    aliases: ["-c", "--config"], description: "Config file path"
  );
  internal static readonly Option<uint?> cellSize = new(
    aliases: ["-s", "--cellsize"], description: "Cell size (in bytes)"
  );
  internal static readonly Option<uint?> columnsLength = new(
    aliases: ["-w", "--width"], description: "Grid columns length"
  );
}
