using System.Diagnostics;

namespace main.ui;

class GridMode
{
  private static readonly List<uint> columnsLengthPatterns = [8, 16, 32, 64];
  private static readonly List<uint> cellLengthPatterns = [1, 2, 4, 8];
  private static uint columnsLength = 8;
  public static uint ColumnsLength
  {
    get => columnsLength;
    set
    {
      if (columnsLengthPatterns.Contains(value))
        columnsLength = value;
      else throw new Exception("Invalid ColumnsLength value");
    }
  }
  private static uint cellLength = 1;
  /// <summary>
  /// The length of the cell in bytes
  /// </summary>
  public static uint CellLength
  {
    get => cellLength;
    set
    {
      if (cellLengthPatterns.Contains(value))
        cellLength = value;
      else throw new Exception("Invalid CellLength value");
    }
  }
}
