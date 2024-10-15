namespace main.operation;
public abstract class IOperation
{
  public virtual string Name => throw new NotImplementedException();
  public virtual void Execute()
  {
    throw new NotImplementedException();
  }
  public virtual Task ExecuteAsync()
  {
    throw new NotImplementedException();
  }
}

public abstract class Operation : IOperation
{
  public override string Name => throw new NotImplementedException();
  public override void Execute()
  {
    base.Execute();
  }
  public override Task ExecuteAsync()
  {
    return base.ExecuteAsync();
  }
}
class Operations
{
  public static IOperation UpdateMemory => new UpdateMemory();
  public static IOperation ChangeMemory => new ChangeMemory();
  public static IOperation Help => new Help();
  public static IOperation Search => new Search();
  public static IOperation Size => new Size();
  public static IOperation Cell => new Cell();
  public static IOperation Columns => new Columns();
  public static IOperation Mark => new Mark();
  public static IOperation Unmark => new Unmark();
  public static IOperation Next => new Next();
  public static IOperation Prev => new Prev();
  public static IOperation Clear => new Clear();
  public static IOperation Jump => new Jump(); 
  public static IOperation Quit => new Quit();
}