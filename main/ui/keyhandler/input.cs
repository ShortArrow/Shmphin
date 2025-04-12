using System.Diagnostics;
using System.Text;

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

public interface IInput
{
  public Action SelectViewDown { get; set; }
  public Action SelectViewUp { get; set; }
  IKeyMap Keymap { get; }
  IKeyMap HelpKeyMap { get; }
  string InputBuffer { get; }
  void InputLoop();
}

public class Input(Operations operations, IMode mode) : IInput
{
  public Action SelectViewDown { get; set; } = () => { };
  public Action SelectViewUp { get; set; } = () => { };
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
  private IKeyMap helpMap => new KeyMap(
    new Dictionary<string, IKeyAction> {
      { "j", new KeyAction("j", "move down", SelectViewDown) },
      { "k", new KeyAction("k", "move up", SelectViewUp) },
      { "q", new KeyAction("q", "quit", () => { mode.InputMode = InputMode.Normal; }) },
      { "Escape", new KeyAction("Escape", "quit", () => { mode.InputMode = InputMode.Normal; }) }
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
    while (!mode.Cts.Token.IsCancellationRequested)
    {
      var key = Console.ReadKey(true);
      IUIMode Handler = mode.InputMode switch
      {
        InputMode.Ex => new CommandHandler(parser.Parse),
        InputMode.NewValue => new NewPropHandler<byte[]>(Parse.NewValue, mode.NewValueTcs),
        InputMode.NewCellSize => new NewPropHandler<uint>(Parse.CellSize, mode.NewCellSizeTcs),
        InputMode.NewColumnsLength => new NewPropHandler<uint>(Parse.ColumnsLength, mode.NewColumnsLengthTcs),
        InputMode.NewSharedMemoryName => new NewPropHandler<string>(Parse.SharedMemoryName, mode.NewSharedMemoryNameTcs),
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
        mode.NewValueTcs?.TrySetCanceled();
        mode.NewColumnsLengthTcs?.TrySetCanceled();
        mode.NewCellSizeTcs?.TrySetCanceled();
        mode.NewSharedMemoryNameTcs?.TrySetCanceled();
        mode.InputMode = InputMode.Normal;
        inputBuffer.Clear();
      }
    }
  }
}
