using main.operation;

namespace main.ui.keyhandler;
public class Normal(Operations operations)
{
  public Action Invoke(ConsoleKeyInfo keyInfo)
  {
    return keyInfo.Key switch
    {
      ConsoleKey.Tab => operations.ChangeFocus.Execute,
      _ => keyInfo.KeyChar.ToString() switch
      {
        "u" => operations.UpdateMemory.Execute,
        "h" => operations.Left.Execute,
        "j" => operations.Down.Execute,
        "k" => operations.Up.Execute,
        "l" => operations.Right.Execute,
        "c" => operations.ChangeMemory.Execute,
        "q" => operations.Quit.Execute,
        "s" => operations.Cell.Execute,
        "n" => operations.Name.Execute,
        ":" => operations.ExCommand.Execute,
        "?" => operations.Help.Execute,
        "/" => operations.Search.Execute,
        _ => () => { }
      }
    };
  }
}
