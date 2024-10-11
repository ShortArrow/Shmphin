class Cell
{
  private uint x;
  private uint y;

  public uint X { get => x; set => x = value; }
  public uint Y { get => y; set => y = value; }
  private byte[] before = new byte[1];
  public byte[] Before { get => before; }
  private byte[] current = new byte[1];
  public byte[] Current
  {
    get => current; set
    {
      before = current;
      current = value;
    }
  }
}