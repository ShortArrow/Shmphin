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
  public static IOperation ShowHelp => new ShowHelp();
  public static IOperation Search => new Search();
}