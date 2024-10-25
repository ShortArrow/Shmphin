using main.operation;

namespace main.ui.keyhandler;
public class NormalKeyEvent(Operations operations)
{
  public IOperation Handling(ConsoleKeyInfo keyInvfo)
  {
    var key = keyInvfo.KeyChar.ToString();
    return key switch
    {
      "u" => operations.UpdateMemory,
      "h" => operations.Left,
      "j" => operations.Down,
      "k" => operations.Up,
      "l" => operations.Right,
      "c" => operations.ChangeMemory,
      "q" => operations.Quit,
      "s" => throw new NotImplementedException(),
      ":" => throw new NotImplementedException(),
      "?" => operations.Help,
      "/" => operations.Search,
      _ => throw new NotSupportedException(),
    };
  }
}
