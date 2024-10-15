namespace main.operation;

public class Parser
{
  public static IOperation Parse(string input)
  {
    var split = input.Split(' ');
    return split[0] switch
    {
      "change" => Operations.ChangeMemory,
      "update" => Operations.UpdateMemory,
      "help" => Operations.ShowHelp,
      "search" => Operations.Search,
      "size" => Operations.Size,
      "columns" => Operations.Columns,
      "cell" => Operations.Cell,
      _ => throw new Exception("Invalid command")
    };
  }
}