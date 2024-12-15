namespace main.model;
public class SelectView(uint max)
{
  private uint maxRow = max;
  public uint MaxRow => maxRow;
  private uint selectedRow = 0;
  public uint SelectedRow => selectedRow;
  public void MoveUp()
  {
    if (selectedRow > 0)
    {
      selectedRow--;
    }
  }
  public void MoveDown()
  {
    if (selectedRow < maxRow - 1)
    {
      selectedRow++;
    }
  }
}
