namespace main.operation;
class Cursor(model.Cursor cursor)
{
  public IOperation Up => new MoveUp(cursor);
  public IOperation Down => new MoveDown(cursor);
  public IOperation Left => new MoveLeft(cursor);
  public IOperation Right => new MoveRight(cursor);
}

class MoveUp(model.Cursor cursor) : Operation()
{
  public override string Name => "moveup";
  public override void Execute()
  {
    cursor.MoveUp();
  }
}
class MoveDown(model.Cursor cursor) : Operation()
{
  public override string Name => "movedown";
  public override void Execute()
  {
    cursor.MoveDown();
  }
}
class MoveLeft(model.Cursor cursor) : Operation()
{
  public override string Name => "moveleft";
  public override void Execute()
  {
    cursor.MoveLeft();
  }
}
class MoveRight(model.Cursor cursor) : Operation()
{
  public override string Name => "moveright";
  public override void Execute()
  {
    cursor.MoveRight();
  }
}
