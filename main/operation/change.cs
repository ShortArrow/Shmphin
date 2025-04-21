using System.Runtime.Versioning;

using main.memory;
using main.ui;

namespace main.operation;
class ChangeMemory(IMemory memory, ISnapShot snapShot, model.ICursor cursor, IMode mode) : IOperation
{
  public string Name => "change";
  [SupportedOSPlatform("windows")]
  public async Task Execute()
  {
    var newValue = await mode.NewValue();
    var index = cursor.GetIndex() ?? 0;
    memory.SharedMemory.Update(index, newValue);
    snapShot.Update();
  }
}
