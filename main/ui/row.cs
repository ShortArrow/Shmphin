class Row
{
  private static Cell[]? items;
  public static Cell[] Items { get => items ?? []; set => items = value; }
}