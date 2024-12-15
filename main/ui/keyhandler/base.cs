namespace main.ui.keyhandler;

public interface IKeyHandler
{
  Action Invoke(ConsoleKeyInfo keyInfo);
  IKeyMap Map { get; }
}

public class KeyAction : IKeyAction
{
  public string DefaultKey { get; }
  public string Description { get; }
  public void Execute() { }
}
