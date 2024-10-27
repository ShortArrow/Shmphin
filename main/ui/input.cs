using System.Text;

using main.operation;
using main.ui.keyhandler;

namespace main.ui;

public enum InputMode
{
  Normal,
  Ex,
  NewValue,
  NewCellSize,
  NewColumnsLength,
  NewSharedMemoryName,
}

public class Input(Operations operations, Mode mode)
{
  private readonly StringBuilder inputBuffer = new();
  public string InputBuffer => inputBuffer.ToString();
  private readonly Parser parser = new(operations);
  public void InputLoop()
  {
    while (!mode.cts.Token.IsCancellationRequested)
    {
      var key = Console.ReadKey(true);
      InputMode modename = mode.InputMode;
      Action Handler = modename switch
      {
        InputMode.Ex => new CommandHandler(
            parser.Parse,
            mode.InputMode,
            inputBuffer
          ).Invoke(key).Invoke,
        InputMode.NewValue => new NewPropHandler<byte[]>(
          Parse.NewValue,
          mode.newValueTcs,
          mode,
          inputBuffer
        ).Invoke(key).Invoke,
        InputMode.NewCellSize => new NewPropHandler<uint>(
            Parse.CellSize,
            mode.newCellSizeTcs,
            mode,
            inputBuffer
          ).Invoke(key).Invoke,
        InputMode.NewColumnsLength => new NewPropHandler<uint>(
            Parse.ColumnsLength,
            mode.newColumnsLengthTcs,
            mode,
            inputBuffer
          ).Invoke(key).Invoke,
        InputMode.NewSharedMemoryName => new NewPropHandler<string>(
          Parse.SharedMemoryName,
          mode.newSharedMemoryNameTcs,
          mode,
          inputBuffer
        ).Invoke(key).Invoke,
        InputMode.Normal => new Normal(operations).Invoke(key).Invoke,
        _ => throw new NotSupportedException(),
      };
      Handler();
    }
  }
}
