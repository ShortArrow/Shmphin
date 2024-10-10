namespace main.ui;

class Cursor
{
  private static int _x = 0;
  private static int _y = 0;
  public static int X
  {
    get => _x;
    set => _x = value;
  }
  public static int Y
  {
    get => _y;
    set => _y = value;
  }
  public static void MoveUp()
  {
    _y--;
  }
  public static void MoveDown()
  {
    _y++;
  }
  public static void MoveLeft()
  {
    _x--;
  }
  public static void MoveRight()
  {
    _x++;
  }
}