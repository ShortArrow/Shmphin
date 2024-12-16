using System.Text;

using main.model;
using main.operation;
using main.ui.mode;

namespace main.ui;

public enum InputMode
{
  Normal,
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
        InputMode.Help => new HelpViewHandler(helpMap).Handler(key).Invoke,
        InputMode.Normal => new Normal(normalMap).Handler(key).Invoke,
        _ => throw new NotSupportedException(),
      };
      Handler();
    }
  }
}
