namespace main.model;
public class Cell(byte[] before, byte[] current, uint x, uint y)
{
  private readonly uint x = x;
  private readonly uint y = y;
  public uint X { get => x; }
  public uint Y { get => y; }
  private readonly byte[] before = before;
  public byte[] Before { get => before; }
  private readonly byte[] current = current;
  public byte[] Current { get => current; }
  public uint Length
  {
    get => ((uint)current.Length > (uint)before.Length)
    ? (uint)current.Length
    : (uint)before.Length;
  }
  public ulong CurrentValue
  {
    get
    {
      return current.Length switch
      {
        1 => current[0],
        2 => BitConverter.ToUInt16(current, 0),
        4 => BitConverter.ToUInt32(current, 0),
        8 => BitConverter.ToUInt64(current, 0),
        _ => 0 // Default case, replace with actual logic
      };
    }
  }
  public ulong BeforeValue
  {
    get
    {
      return before.Length switch
      {
        1 => before[0],
        2 => BitConverter.ToUInt16(before, 0),
        4 => BitConverter.ToUInt32(before, 0),
        8 => BitConverter.ToUInt64(before, 0),
        _ => 0 // Default case, replace with actual logic
      };
    }
  }
}
