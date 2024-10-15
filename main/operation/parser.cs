namespace main.operation;

public class Parser
{
  public static IOperation Parse(string input)
  {
    var split = input.Split(' ');
    return split[0] switch
    {
      "change" => Operation.ChangeMemory,
      "update" => Operation.UpdateMemory,
      _ => throw new Exception("Invalid command")
    };
  }
}