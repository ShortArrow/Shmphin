using main.config;
using main.memory;

namespace main.model;
public class Matrix(ICurrentConfig config, ISnapShot snapShot)
{
  private Cell[]? cells;
  private uint cellSize = config.CellLength ?? 1;
  public uint Length { get => (uint)(cells?.Length ?? 0); }
  private uint width = config.ColumnsLength ?? 8;
  public uint Width { get => width; }
  private uint height = 0;
  public uint Height { get => height; }
  public uint LastIndex { get => (uint)((cells?.Length ?? 0) * (config.CellLength ?? 1) - 1); }
  public void Update(byte[]? before = null, byte[]? current = null)
  {
    before ??= snapShot.Before;
    current ??= snapShot.Current;
    cellSize = config.CellLength ?? throw new Exception("CellLength not set");
    width = config.ColumnsLength ?? throw new Exception("ColumnsLength not set");
    cells = new Cell[current.Length / cellSize];
    height = (Length == 0) ? 0 : ((uint)current.Length / cellSize / width);
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
    // Check if the coordinates are within bounds
    if (x >= width || y >= height)
      throw new Exception($"Cell({x}, {y}) is out of bounds (width={width}, height={height}).");
    uint index = y * width + x; // Calculate index based on row-major order
    return cells[index];
  }
  public Cell[] GetColumn(uint x)
  {
    if (cells == null) throw new Exception("Matrix not initialized");
    return [.. cells.Where(cell => cell.X == x)];
  }
}
