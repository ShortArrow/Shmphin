using System.Diagnostics;

namespace main.ui;

class Cursor
{
  private static uint x = 0;
  private static uint y = 0;
  public static uint X
  {
    get => x;
    set => x = value;
  }
  public static uint Y
  {
    get => y;
    set => y = value;
  }
  public static void MoveUp()
  {
    Debug.WriteLine($"move up: x: {x}, y: {y}");
    if (y > 0) { y--; }
  }
  public static void MoveDown()
  {
    Debug.WriteLine($"move down: x: {x}, y: {y}");
    var height = memory.Params.Size / (GridMode.ColumnsLength * GridMode.CellLength);
    if (y < height - 1) { y++; }
  }
  public static void MoveLeft()
  {
    Debug.WriteLine($"move left: x: {x}, y: {y}");
    if (x > 0) { x--; }
  }
  public static void MoveRight()
  {
    Debug.WriteLine($"move right: x: {x}, y: {y}");
    if (X < GridMode.ColumnsLength - 1) { x++; }
  }
}