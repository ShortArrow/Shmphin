using System.Runtime.Versioning;

namespace main.operation;
class UpdateMemory : Operation
{
  public override string Name => "update";
  [SupportedOSPlatform("windows")]
  public override void Execute()
  {
    memory.SnapShot.UpdateSnapShot();
  }
}