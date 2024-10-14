using System.Diagnostics;
using main.ui;

namespace main.model;
public class Matrix
{
  private Cell[]? cells;
  public uint Length { get => (uint)(cells?.Length ?? 0); }
  public uint Width { get => GridMode.ColumnsLength; }
  public uint Height
  {
    get => (Length == 0)
      ? 0
      : (Length / GridMode.CellLength / GridMode.ColumnsLength);
  }
  public void Update(byte[]? before = null, byte[]? current = null)
  {
    before ??= memory.SnapShot.Before;
    current ??= memory.SnapShot.Current;
    cells = new Cell[current.Length / GridMode.CellLength];
    uint index = 0;
    uint cellLength = GridMode.CellLength;
    for (uint i = 0; i < current.Length; i += cellLength)
    {
      var beforeSegment = new ArraySegment<byte>(before, (int)i, (int)cellLength).ToArray();
      var currentSegment = new ArraySegment<byte>(current, (int)i, (int)cellLength).ToArray();
      uint position = i / cellLength;
      uint x = position % GridMode.ColumnsLength;
      uint y = position / GridMode.ColumnsLength;
      Debug.WriteLine($"Cell created at X: {x}, Y: {y}");
      cells[index++] = new Cell(
        beforeSegment,
        currentSegment,
        x,
        y
      );
    }
  }
  public Cell GetCell(uint x, uint y)
  {
    if (cells == null) throw new Exception("Matrix not initialized");
    Cell cell = cells.Where(cell => cell.X == x && cell.Y == y).FirstOrDefault()
      ?? throw new Exception($"Cell({x}, {y}) not found");
    return cell;
  }
  public Cell[] GetColumn(uint x)
  {
    if (cells == null) throw new Exception("Matrix not initialized");
    return cells.Where(cell => cell.X == x).ToArray();
  }
}