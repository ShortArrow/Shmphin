namespace main.model;
public class Cell(byte[] before, byte[] current, uint x, uint y)
{
  private uint x = x;
  private uint y = y;
  public uint X { get => x; }
  public uint Y { get => y; }
  private byte[] before = before;
  public byte[] Before { get => before; }
  private byte[] current = current;
  public byte[] Current { get => current; }
}