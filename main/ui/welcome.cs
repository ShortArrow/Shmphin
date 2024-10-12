using Spectre.Console;

namespace main.ui;

class Welcome

{
  private static bool isShown = false;
  public static void Show()
  {
    if (isShown) return;
    AnsiConsole.Write(new FigletText("Shmphin").LeftJustified().Color(Color.Green));
    AnsiConsole.MarkupLine($"Welcome! [bold green]Shmphin[/] is a shared memory editor.");
    isShown = true;
  }
}