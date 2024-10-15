using System.Runtime.Versioning;

namespace main.operation;
class UpdateMemory : Operation
{
  [SupportedOSPlatform("windows")]
  public override void Execute()
  {
    memory.SnapShot.UpdateSnapShot();
  }
}