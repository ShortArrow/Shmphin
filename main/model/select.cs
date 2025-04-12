using main.ui.keyhandler;

namespace main.model;

public interface ISelectView
{
  uint MaxRow { get; }
  uint SelectedRow { get; }
  void MoveUp();
  void MoveDown();
}

public class SelectView : ISelectView
{
  private readonly IInput input;

  public SelectView(IInput input)
  {
    this.input = input;
    input.SelectViewDown = MoveDown;
    input.SelectViewUp = MoveUp;
  }
  public uint MaxRow => (uint)(input?.HelpKeyMap.List.Count ?? 0);
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
