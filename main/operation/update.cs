using System.Runtime.Versioning;

using main.memory;

namespace main.operation;
class UpdateMemory(Memory memory) : Operation()
{
  public override string Name => "update";
  [SupportedOSPlatform("windows")]
  public override void Execute()
  {
    memory.SnapShot.Update();
  }
}
