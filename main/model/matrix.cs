using main.config;
using main.ui;

namespace main.model;
public class Matrix(IConfig config)
{
  private Cell[]? cells;
  private uint cellSize = GridMode.CellLength;
  public uint Length { get => (uint)(cells?.Length ?? 0); }
  private uint width = GridMode.ColumnsLength;
  public uint Width { get => width; }
  private uint height = 0;
  public uint Height { get => height; }
  public void Update(byte[]? before = null, byte[]? current = null)
  {
    before ??= new memory.SnapShot(config).Before;
    current ??= new memory.SnapShot(config).Current;
    cells = new Cell[current.Length / GridMode.CellLength];
    cellSize = GridMode.CellLength;
    width = GridMode.ColumnsLength;
    height = (Length == 0) ? 0 : (Length / cellSize / width);
    uint index = 0;
    for (uint i = 0; i < current.Length; i += cellSize)
    {
      var beforeSegment = new ArraySegment<byte>(before, (int)i, (int)cellSize).ToArray();
      var currentSegment = new ArraySegment<byte>(current, (int)i, (int)cellSize).ToArray();
      uint position = i / cellSize;
      uint x = position % width;
      uint y = position / width;
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
  public static ulong GetIndex(uint x, uint y)
  {
    return y * GridMode.ColumnsLength * GridMode.CellLength + x;
  }
}
