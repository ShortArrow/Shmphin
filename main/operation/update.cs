using System.Runtime.Versioning;

using main.memory;

namespace main.operation;
class UpdateMemory(SnapShot snapShot) : Operation()
{
  public override string Name => "update";
  [SupportedOSPlatform("windows")]
  public override void Execute()
  {
    snapShot.Update();
  }
}
