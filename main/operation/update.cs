using System.Runtime.Versioning;

using main.memory;

namespace main.operation;
class UpdateMemory(ISnapShot snapShot) : IOperation
{
  public string Name => "update";
  [SupportedOSPlatform("windows")]
  public Task Execute()
  {
    snapShot.Update();
    return Task.CompletedTask;
  }
}
