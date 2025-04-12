namespace main;

public interface IConsole
{
  void WriteLine(string message);
}

internal sealed class DefaultConsole : IConsole
{
  public bool IsEnabled { get; set; } = true;

  void IConsole.WriteLine(string message)
  {
    if (IsEnabled is false)
    {
      return;
    }

    Console.WriteLine(message);
  }
}
