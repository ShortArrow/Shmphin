namespace main.model;
public class SelectView(Func<uint> getMax)
{
  public uint MaxRow => getMax();
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
    if (selectedRow < MaxRow - 1)
    {
      selectedRow++;
    }
  }
}
