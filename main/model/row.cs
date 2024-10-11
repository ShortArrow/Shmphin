class Row
{
  private Cell[]? items;
  public Cell[] Items { get => items ?? []; set => items = value; }
}