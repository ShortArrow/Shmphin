namespace main.operation;

public class Parser
{
  public static IOperation Parse(string input)
  {
    var split = input.Split(' ');
    return split[0] switch
    {
      "change" => Operations.ChangeMemory, // change memory value at current cell
      "update" => Operations.UpdateMemory, // update current memory snapshot
      "help" => Operations.Help, // display help
      "search" => Operations.Search, // search for a byte sequence
      "size" => Operations.Size, // update size of the memory displayed
      "columns" => Operations.Columns, // update quantity of columns displayed
      "cell" => Operations.Cell, // update cell byte size displayed
      "mark" => Operations.Mark, // book mark the current cell
      "unmark" => Operations.Unmark, // remove the book mark from the current cell
      "jump" => Operations.Jump, // jump to the book marked cell
      "clear" => Operations.Clear, // clear all book marks
      "next" => Operations.Next, // move to the next cell search result
      "prev" => Operations.Prev, // move to the previous cell search result
      "exit" => Operations.Quit, // exit the program
      "quit" => Operations.Quit, // exit the program
      _ => throw new Exception("Invalid command")
    };
  }
}