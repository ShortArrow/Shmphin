using Spectre.Console;

namespace main.ui;

public class Prompt
{
  public static string PromptHeader(InputMode inputMode)
  {
    return inputMode switch
    {
      InputMode.Normal => "Normal",
      InputMode.Ex => ">",
      InputMode.NewValue => "New value :",
      InputMode.NewCellSize => "New cell size :",
      InputMode.NewColumnsLength => "New columns length :",
      InputMode.NewSharedMemoryName => "New shared memory name :",
      _ => "Unknown mode"
    };
  }
  public static Markup ShowInput(string inputBuffer, InputMode inputMode)
  {
    var header = PromptHeader(inputMode);
    return new Markup($"[green]{header}{inputBuffer}[/]");
  }
}
