using System.Runtime.Versioning;

namespace main.operation;
class UpdateMemory : IOperation
{
  [SupportedOSPlatform("windows")]
  public override void Execute()
  {
    memory.SnapShot.UpdateSnapShot();
  }
}