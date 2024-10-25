using main.operation;

namespace main.ui.keyhandler;
public class Normal(Operations operations)
{
  public IOperation Handling(ConsoleKeyInfo keyInfo)
  {
    return keyInfo.Key switch
    {
      ConsoleKey.Tab => operations.ChangeFocus,
      _ => keyInfo.KeyChar.ToString() switch
      {
        "u" => operations.UpdateMemory,
        "h" => operations.Left,
        "j" => operations.Down,
        "k" => operations.Up,
        "l" => operations.Right,
        "c" => operations.ChangeMemory,
        "q" => operations.Quit,
        "s" => throw new NotImplementedException(),
        ":" => operations.ExCommand,
        "?" => operations.Help,
        "/" => operations.Search,
        _ => throw new NotSupportedException(),
      }
    };
  }
}
