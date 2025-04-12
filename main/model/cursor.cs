using main.config;

using System.Diagnostics;

namespace main.model;

public interface ICursor
{
  public uint X { get; set; }
  public uint Y { get; set; }
  public void MoveUp();
  public void MoveDown();
  public void MoveLeft();
  public void MoveRight();
  public uint? GetIndex();
}

public class Cursor(IConfig config) : ICursor
{
  private uint x = 0;
  private uint y = 0;
  public uint X
  {
    get => x;
    set => x = value;
  }
  public uint Y
  {
    get => y;
    set => y = value;
  }
  public void MoveUp()
  {
    Debug.WriteLine($"move up: x: {x}, y: {y}");
    if (y > 0) { y--; }
    else { y = 0; }
  }
  public void MoveDown()
  {
    Debug.WriteLine($"move down: x: {x}, y: {y}");
    uint height = (config.SharedMemorySize / (config.ColumnsLength * config.CellLength)) ?? 0;
    if (y < height - 1) { y++; }
    else { y = 0; MoveRight(); }
  }
  public void MoveLeft()
  {
    Debug.WriteLine($"move left: x: {x}, y: {y}");
    if (x > 0) { x--; }
    else { x = 0; }
  }
  public void MoveRight()
  {
    Debug.WriteLine($"move right: x: {x}, y: {y}");
    if (x < config.ColumnsLength - 1) { x++; }
    else { x = 0; MoveDown(); }
  }
  public uint? GetIndex()
  {
    return y * config.ColumnsLength * config.CellLength + x;
  }
}
