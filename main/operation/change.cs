using System.Runtime.Versioning;

using main.ui;

namespace main.operation;
class ChangeMemory(memory.Memory memory, model.Cursor cursor, Mode mode) : Operation()
{
  public override string Name => "change";
  [SupportedOSPlatform("windows")]
  public override async void Execute()
  {
    var newValue = await mode.NewValue();
    var index = cursor.GetIndex() ?? 0;
    memory.SharedMemory.Update(index, newValue);
    memory.SnapShot.Update();
  }
}
