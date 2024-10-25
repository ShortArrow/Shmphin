using System.Runtime.Versioning;

using main.ui;

namespace main.operation;
class ChangeMemory(memory.Memory memory, model.Cursor cursor, Input input) : Operation()
{
  public override string Name => "change";
  [SupportedOSPlatform("windows")]
  public override async void Execute()
  {
    var newValue = await input.NewValue();
    var index = cursor.GetIndex() ?? 0;
    memory.SharedMemory.Update(index, newValue);
    memory.SnapShot.Update();
  }
}
