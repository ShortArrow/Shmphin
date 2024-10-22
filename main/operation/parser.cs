namespace main.operation;

public class Parser(Operations operations)
{
  public IOperation Parse(string input)
  {
    var split = input.Split(' ');
    return split[0] switch
    {
      // "change" => operations.ChangeMemory, // change memory value at current cell
      // "update" => operations.UpdateMemory, // update current memory snapshot
      // "help" => operations.Help, // display help
      // "search" => operations.Search, // search for a byte sequence
      // "size" => operations.Size, // update size of the memory displayed
      // "columns" => operations.Columns, // update quantity of columns displayed
      // "cell" => operations.Cell, // update cell byte size displayed
      // "mark" => operations.Mark, // book mark the current cell
      // "unmark" => operations.Unmark, // remove the book mark from the current cell
      // "jump" => operations.Jump, // jump to the book marked cell
      // "clear" => operations.Clear, // clear all book marks
      // "next" => operations.Next, // move to the next cell search result
      // "prev" => operations.Prev, // move to the previous cell search result
      // "exit" => operations.Quit, // exit the program
      // "quit" => operations.Quit, // exit the program
      _ => throw new Exception("Invalid command")
    };
  }
}
