namespace main.ui.keyhandler;

public interface IKeyAction
{
  public string DefaultKey { get; }
  public string Description { get; }
  void Execute();
}
public interface IKeyMap
{
  Dictionary<string, IKeyAction> List { get; }
}
