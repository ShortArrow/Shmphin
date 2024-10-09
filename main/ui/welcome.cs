using Spectre.Console;

namespace main.ui;

class Welcome

{
  public static void Show()
  {
    AnsiConsole.Write(new FigletText("Shmphin").LeftJustified().Color(Color.Green));
    AnsiConsole.MarkupLine($"Welcome! [bold green]Shmphin[/] is a shared memory editor.");
  }
}