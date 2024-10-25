using System.Diagnostics;

namespace main.ui;

public class Mode
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
  private InputMode mode = InputMode.Normal;
  public InputMode InputMode
  {
    get => mode;
    set => mode = value;
  }
  public readonly CancellationTokenSource cts = new();
  public bool IsCancellationRequested => cts.Token.IsCancellationRequested;
  public TaskCompletionSource<byte[]>? newValueTcs;
  public TaskCompletionSource<uint>? newCellSizeTcs;
  public TaskCompletionSource<uint>? newColumnsLengthTcs;
  public Task<byte[]> NewValue()
  {
    mode = InputMode.NewValue;
    Debug.WriteLine("New value mode");
    newValueTcs = new TaskCompletionSource<byte[]>();
    return newValueTcs.Task;
  }
  public Task<uint> NewCellSize()
  {
    mode = InputMode.NewCellSize;
    Debug.WriteLine("New cell size mode");
    newCellSizeTcs = new TaskCompletionSource<uint>();
    return newCellSizeTcs.Task;
  }
  public Task<uint> NewColumnsLength()
  {
    mode = InputMode.NewColumnsLength;
    Debug.WriteLine("New columns length mode");
    newColumnsLengthTcs = new TaskCompletionSource<uint>();
    return newColumnsLengthTcs.Task;
  }
}
