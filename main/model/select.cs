using main.ui;
using main.ui.keyhandler;

namespace main.model;

public interface ISelectView
{
  uint MaxRow { get; }
  uint SelectedRow { get; }
  void MoveUp();
  void MoveDown();
  void Select();
  void Escape();
}

public class SelectView : ISelectView
{
  private readonly IInput input;
  private readonly IMode mode;

  public SelectView(IInput input, IMode mode)
  {
    this.input = input;
    this.mode = mode;
    input.SelectViewDown = MoveDown;
    input.SelectViewUp = MoveUp;
    input.SelectViewSelect = Select;
  }
  public uint MaxRow => (uint)(input?.HelpKeyMap.List.Count ?? 0);
  private uint selectedRow = 0;
  public uint SelectedRow => selectedRow;
  public void Select()
  {
    Escape();
    input.HelpKeyMap.List.ElementAt((int)selectedRow).Value.Execute();
  }
  public void Escape()
  {
    mode.InputMode = InputMode.Normal;
  }
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
