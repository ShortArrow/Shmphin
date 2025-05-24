using main.config;
using main.memory;
using main.ui;

namespace main.operation;
public interface IOperation
{
  string Name { get; }
  Task Execute();
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
  IOperation ScrollUp { get; }
  IOperation ScrollDown { get; }
}

public class Operations(
  ICurrentConfig config,
  model.ICursor cursor,
  IMemory memory,
  ISnapShot snapShot,
  IMode mode,
  IFocus focus,
  Func<main.ui.layout.MainGrid> getMainGridFunc
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
  public IOperation Up => new Cursor(cursor, getMainGridFunc).Up;
  public IOperation Down => new Cursor(cursor, getMainGridFunc).Down;
  public IOperation Left => new Cursor(cursor, getMainGridFunc).Left;
  public IOperation Right => new Cursor(cursor, getMainGridFunc).Right;
  public IOperation ExCommand => new ExCommand(mode);
  public IOperation ChangeFocus => focus.ChangeFocus;
  public IOperation ScrollUp => new ScrollUpOperation(getMainGridFunc);
  public IOperation ScrollDown => new ScrollDownOperation(getMainGridFunc);
}
