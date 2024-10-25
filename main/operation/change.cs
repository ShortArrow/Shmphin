using System.Runtime.Versioning;

using main.memory;
using main.ui;

namespace main.operation;
class ChangeMemory(Memory memory, SnapShot snapShot, model.Cursor cursor, Mode mode) : Operation()
{
  public override string Name => "change";
  [SupportedOSPlatform("windows")]
  public override async void Execute()
  {
    var newValue = await mode.NewValue();
    var index = cursor.GetIndex() ?? 0;
    memory.SharedMemory.Update(index, newValue);
    snapShot.Update();
  }
}
