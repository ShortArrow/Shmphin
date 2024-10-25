using System.Diagnostics;

using main.operation;

namespace main.ui.keyhandler;
public class NormalKeyEvent(Operations operations)
{
  public IOperation Handling(ConsoleKeyInfo keyInvfo)
  {
    var key = keyInvfo.KeyChar.ToString();
    if (keyInvfo.Key == ConsoleKey.Tab)
    {
      Debug.WriteLine("Tab key is pressed.");
      return operations.ChangeFocus;
    }
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
      ":" => operations.ExCommand,
      "?" => operations.Help,
      "/" => operations.Search,
      _ => throw new NotSupportedException(),
    };
  }
}
