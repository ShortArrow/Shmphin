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
}