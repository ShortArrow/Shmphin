namespace main.operation;
public abstract class IOperation
{
  public virtual void Execute()
  {
    throw new NotImplementedException();
  }
}

class Operation
{
  public static IOperation UpdateMemory => new UpdateMemory();
  public static IOperation ChangeMemory => new ChangeMemory();
  public static IOperation ShowHelp => new ShowHelp();
  public static IOperation Search => new Search();
}