namespace main.operation;
class Cursor(model.ICursor cursor)
{
  public IOperation Up => new MoveUp(cursor);
  public IOperation Down => new MoveDown(cursor);
  public IOperation Left => new MoveLeft(cursor);
  public IOperation Right => new MoveRight(cursor);
}

class MoveUp(model.ICursor cursor) : IOperation
{
  public string Name => "moveup";
  public Task Execute()
  {
    cursor.MoveUp();
    return Task.CompletedTask;
  }
}
class MoveDown(model.ICursor cursor) : IOperation
{
  public string Name => "movedown";
  public Task Execute()
  {
    cursor.MoveDown();
    return Task.CompletedTask;
  }
}
class MoveLeft(model.ICursor cursor) : IOperation
{
  public string Name => "moveleft";
  public Task Execute()
  {
    cursor.MoveLeft();
    return Task.CompletedTask;
  }
}
class MoveRight(model.ICursor cursor) : IOperation
{
  public string Name => "moveright";
  public Task Execute()
  {
    cursor.MoveRight();
    return Task.CompletedTask;
  }
}
