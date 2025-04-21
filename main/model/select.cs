using main.ui;
using main.ui.keyhandler;

namespace main.model;

public interface ISelectView
{
  uint MaxRow { get; }
  uint SelectedRow { get; }
  Task MoveUp();
  Task MoveDown();
  Task Select();
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
  public Task Select()
  {
    Escape();
    input.HelpKeyMap.List.ElementAt((int)selectedRow).Value.Execute();
    return Task.CompletedTask;
  }
  public void Escape()
  {
    mode.InputMode = InputMode.Normal;
  }
  public Task MoveUp()
  {
    if (selectedRow > 0)
    {
      selectedRow--;
    }
    return Task.CompletedTask;
  }
  public Task MoveDown()
  {
    if (selectedRow < MaxRow - 1)
    {
      selectedRow++;
    }
    return Task.CompletedTask;
  }
}
