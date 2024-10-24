using System.Runtime.Versioning;

using main.config;

namespace main.operation;
class UpdateMemory(IConfig config) : Operation()
{
  public override string Name => "update";
  [SupportedOSPlatform("windows")]
  public override void Execute()
  {
    memory.Memory memory = new(config);
    memory.SnapShot.Update();
  }
}
