using System.Data.Common;
using System.Diagnostics;
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
  NewColumnsLength
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
      switch (mode.InputMode)
      {
        case InputMode.Ex:
          var ExHandler = new CommandHandler(
            parser.Parse,
            mode.InputMode,
            inputBuffer
          );
          ExHandler.Invoke(key).Invoke();
          break;
        case InputMode.NewValue:
          var newValueHandler = new NewPropHandler<byte[]>(
            Parse.NewValue,
            mode.newValueTcs,
            mode.InputMode,
            inputBuffer
          );
          break;
        case InputMode.NewCellSize:
          var newCellSizeHandler = new NewPropHandler<uint>(
            Parse.CellSize,
            mode.newCellSizeTcs,
            mode.InputMode,
            inputBuffer
          );
          newCellSizeHandler.Invoke(key).Invoke();
          break;
        case InputMode.NewColumnsLength:
          var newColumnsLengthHandler = new NewPropHandler<uint>(
            Parse.ColumnsLength,
            mode.newColumnsLengthTcs,
            mode.InputMode,
            inputBuffer
          );
          newColumnsLengthHandler.Invoke(key).Invoke();
          break;
        case InputMode.Normal:
          var handler = new Normal(operations);
          handler.Handling(key).Execute();
          break;
        default:
          break;
      }
    }
  }
}
