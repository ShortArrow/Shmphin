using main.config;
using main.memory;
using main.ui;

namespace main.operation;
public abstract class IOperation()
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

public abstract class Operation() : IOperation()
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
public class Operations(CurrentConfig config, model.Cursor cursor, Memory memory, SnapShot snapShot, Mode mode, Focus focus)
{
  public IOperation UpdateMemory => new UpdateMemory(snapShot);
  public IOperation ChangeMemory => new ChangeMemory(memory, snapShot, cursor, mode);
  public IOperation Help => new Help();
  public IOperation Search => new Search();
  public IOperation Size => new Size();
  public IOperation Cell => new Cell(config, mode);
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
