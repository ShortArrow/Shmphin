using System.Runtime.Versioning;
using main.model;
using main.ui;

namespace main.operation;
class ChangeMemory : Operation
{
  public override string Name => "change";
  [SupportedOSPlatform("windows")]
  public override async void Execute()
  {
    var newValue = await Input.NewValue();
    Matrix matrix = new();
    matrix.Update();
    var index = Matrix.GetIndex(Cursor.X, Cursor.Y);
    memory.SharedMemory.Update(index, newValue);
    memory.SnapShot.UpdateSnapShot();
  }
}
