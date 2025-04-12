using System.Diagnostics;

using main.ui.keyhandler;

namespace main.ui;

public interface IMode
{
  CancellationTokenSource Cts { get; }
  InputMode PreviousMode { get; }
  InputMode InputMode { get; set; }
  bool IsCancellationRequested { get; }
  TaskCompletionSource<byte[]>? NewValueTcs { get; }
  TaskCompletionSource<uint>? NewCellSizeTcs { get; }
  TaskCompletionSource<uint>? NewColumnsLengthTcs { get; }
  TaskCompletionSource<string>? NewSharedMemoryNameTcs { get; }
  Task<byte[]> NewValue();
  Task<uint> NewCellSize();
  Task<uint> NewColumnsLength();
  Task<string> NewSharedMemoryName();
}

public class Mode : IMode
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
  private InputMode previousMode = InputMode.Normal;
  public InputMode PreviousMode => previousMode;
  private InputMode mode = InputMode.Normal;
  public InputMode InputMode
  {
    get => mode;
    set
    {
      previousMode = mode;
      mode = value;
    }
  }
  public CancellationTokenSource Cts { get; private set; } = new();
  public bool IsCancellationRequested => Cts.Token.IsCancellationRequested;
  public TaskCompletionSource<byte[]>? NewValueTcs { get; set; }
  public TaskCompletionSource<uint>? NewCellSizeTcs { get; set; }
  public TaskCompletionSource<uint>? NewColumnsLengthTcs { get; set; }
  public TaskCompletionSource<string>? NewSharedMemoryNameTcs { get; set; }
  public Task<byte[]> NewValue()
  {
    mode = InputMode.NewValue;
    Debug.WriteLine("New value mode");
    NewValueTcs = new TaskCompletionSource<byte[]>();
    return NewValueTcs.Task;
  }
  public Task<uint> NewCellSize()
  {
    mode = InputMode.NewCellSize;
    Debug.WriteLine("New cell size mode");
    NewCellSizeTcs = new TaskCompletionSource<uint>();
    return NewCellSizeTcs.Task;
  }
  public Task<uint> NewColumnsLength()
  {
    mode = InputMode.NewColumnsLength;
    Debug.WriteLine("New columns length mode");
    NewColumnsLengthTcs = new TaskCompletionSource<uint>();
    return NewColumnsLengthTcs.Task;
  }
  public Task<string> NewSharedMemoryName()
  {
    mode = InputMode.NewSharedMemoryName;
    Debug.WriteLine("New shared memory name mode");
    NewSharedMemoryNameTcs = new TaskCompletionSource<string>();
    return NewSharedMemoryNameTcs.Task;
  }
}
