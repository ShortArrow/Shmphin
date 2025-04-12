using main.config;
using main.memory;
using main.ui;

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

public interface IOperations
{
  IOperation UpdateMemory { get; }
  IOperation ChangeMemory { get; }
  IOperation Help { get; }
  IOperation Search { get; }
  IOperation Size { get; }
  IOperation Cell { get; }
  IOperation Name { get; }
  IOperation Columns { get; }
  IOperation Mark { get; }
  IOperation Unmark { get; }
  IOperation Next { get; }
  IOperation Prev { get; }
  IOperation Clear { get; }
  IOperation Jump { get; }
  IOperation Quit { get; }
  IOperation Up { get; }
  IOperation Down { get; }
  IOperation Left { get; }
  IOperation Right { get; }
  IOperation ExCommand { get; }
  IOperation ChangeFocus { get; }
}

public class Operations(
  ICurrentConfig config,
  model.ICursor cursor,
  IMemory memory,
  ISnapShot snapShot,
  IMode mode,
  IFocus focus
) : IOperations
{
  public IOperation UpdateMemory => new UpdateMemory(snapShot);
  public IOperation ChangeMemory => new ChangeMemory(memory, snapShot, cursor, mode);
  public IOperation Help => new Help(mode);
  public IOperation Search => new Search();
  public IOperation Size => new Size();
  public IOperation Cell => new Cell(config, mode);
  public IOperation Name => new SharedMemoryName(config, mode);
  public IOperation Columns => new Columns(config, mode);
  public IOperation Mark => new Mark();
  public IOperation Unmark => new Unmark();
  public IOperation Next => new Next();
  public IOperation Prev => new Prev();
  public IOperation Clear => new Clear();
  public IOperation Jump => new Jump();
  public IOperation Quit => new Quit(mode);
  public IOperation Up => new Cursor(cursor).Up;
  public IOperation Down => new Cursor(cursor).Down;
  public IOperation Left => new Cursor(cursor).Left;
  public IOperation Right => new Cursor(cursor).Right;
  public IOperation ExCommand => new ExCommand(mode);
  public IOperation ChangeFocus => focus.ChangeFocus;
}
