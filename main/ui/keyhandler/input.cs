using System.Diagnostics;
using System.Text;

using main.model;
using main.operation;
using main.ui.mode;

namespace main.ui.keyhandler;

public enum InputMode
{
  Normal,
  Search,
  Ex,
  NewValue,
  NewCellSize,
  NewColumnsLength,
  NewSharedMemoryName,
  Help,
}

public class Input(Operations operations, Mode mode, SelectView selectView)
{
  public SelectView SelectView => selectView;
  private readonly KeyMap normalMap = new(
    new Dictionary<string, IKeyAction> {
      { "h", new KeyAction("h", "move left", operations.Left.Execute) },
      { "j", new KeyAction("j", "move down", operations.Down.Execute) },
      { "k", new KeyAction("k", "move up", operations.Up.Execute) },
      { "l", new KeyAction("l", "move right", operations.Right.Execute) },
      { "c", new KeyAction("c", "change memory", operations.ChangeMemory.Execute) },
      { "q", new KeyAction("q", "quit", operations.Quit.Execute) },
      { "s", new KeyAction("s", "cell", operations.Cell.Execute) },
      { "n", new KeyAction("n", "name", operations.Name.Execute) },
      { ":", new KeyAction(":", "ex command", operations.ExCommand.Execute) },
      { "?", new KeyAction("?", "help", operations.Help.Execute) },
      { "/", new KeyAction("/", "search", operations.Search.Execute) },
      { "Tab", new KeyAction("Tab", "change focus", operations.ChangeFocus.Execute) }
    }
  );
  private readonly IKeyMap helpMap = new KeyMap(
    new Dictionary<string, IKeyAction> {
      { "j", new KeyAction("j", "move down", selectView.MoveDown) },
      { "k", new KeyAction("k", "move up", selectView.MoveUp) },
      { "q", new KeyAction("q", "quit", ()=> {mode.InputMode = InputMode.Normal;} ) },
      { "Escape", new KeyAction("Escape", "quit", ()=> {mode.InputMode = InputMode.Normal;} ) }
  });
  public IKeyMap Keymap => mode.InputMode switch
  {
    InputMode.Normal => normalMap,
    InputMode.Help => helpMap,
    _ => normalMap
  };
  public IKeyMap HelpKeyMap => mode.PreviousMode switch
  {
    InputMode.Normal => normalMap,
    InputMode.Help => normalMap,
    _ => normalMap
  };
  private readonly StringBuilder inputBuffer = new();
  public string InputBuffer => inputBuffer.ToString();
  private readonly Parser parser = new(operations);
  public void InputLoop()
  {
    while (!mode.cts.Token.IsCancellationRequested)
    {
      var key = Console.ReadKey(true);
      IUIMode Handler = mode.InputMode switch
      {
        InputMode.Ex => new CommandHandler(parser.Parse),
        InputMode.NewValue => new NewPropHandler<byte[]>(Parse.NewValue, mode.newValueTcs),
        InputMode.NewCellSize => new NewPropHandler<uint>(Parse.CellSize, mode.newCellSizeTcs),
        InputMode.NewColumnsLength => new NewPropHandler<uint>(Parse.ColumnsLength, mode.newColumnsLengthTcs),
        InputMode.NewSharedMemoryName => new NewPropHandler<string>(Parse.SharedMemoryName, mode.newSharedMemoryNameTcs),
        InputMode.Help => new HelpViewHandler(helpMap),
        InputMode.Normal => new Normal(normalMap),
        _ => throw new NotSupportedException(),
      };
      try
      {
        var (Invoke, newMode) = Handler.SelectAction(mode.InputMode, inputBuffer, key);
        Invoke();
        if (newMode != null)
        {
          mode.InputMode = newMode.Value;
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.ToString());
        mode.newValueTcs?.TrySetCanceled();
        mode.newColumnsLengthTcs?.TrySetCanceled();
        mode.newCellSizeTcs?.TrySetCanceled();
        mode.newSharedMemoryNameTcs?.TrySetCanceled();
        mode.InputMode = InputMode.Normal;
        inputBuffer.Clear();
      }
    }
  }
}
