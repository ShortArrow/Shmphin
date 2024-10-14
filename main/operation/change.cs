using System.Runtime.Versioning;
using main.model;
using main.ui;

namespace main.operation;
class ChangeMemory : IOperation
{
  [SupportedOSPlatform("windows")]
  public override void Execute()
  {
    var task = Input.NewValue();
    task.Start();
    task.Wait();
    byte[] newValue = task.Result;
    Matrix matrix = new();
    matrix.Update();
    var index = matrix.GetIndex(Cursor.X, Cursor.Y);
    memory.SharedMemory.Update(index, newValue);
    memory.SnapShot.UpdateSnapShot();
  }
}
