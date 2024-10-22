using System.Runtime.Versioning;

using main.model;
using main.ui;

namespace main.operation;
class ChangeMemory(memory.Memory memory, ui.Cursor cursor, Input input) : Operation()
{
  public override string Name => "change";
  [SupportedOSPlatform("windows")]
  public override async void Execute()
  {
    var newValue = await input.NewValue();
    var index = Matrix.GetIndex(cursor.X, cursor.Y);
    memory.SharedMemory.Update(index, newValue);
    memory.SnapShot.UpdateSnapShot();
  }
}
