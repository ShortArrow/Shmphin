public interface IKeyAction
{
  public string DefaultKey { get; }
  public string Description { get; }
  void Execute();
}
public interface IKeyMap
{
  Dictionary<string,IKeyAction> List { get; }
}
public class KeyMaps
{
  public readonly Dictionary<string, string> list = new(
    [
      new("u", "update display with read from memory"),
      new(":", "command mode"),
      new("q", "quit"),
      new("?", "help"),
      new("/", "search"),
      new("j", "move cursor down"),
      new("k", "move cursor up"),
      new("l", "move cursor right"),
      new("h", "move cursor left"),
      new("c", "change memory with input")
    ]
  );
}
