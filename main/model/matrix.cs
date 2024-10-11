namespace main.model;
class Matrix
{
  private static Row[]? rows;
  public static Row[] Rows { get => rows ?? []; set => rows = value; }
}