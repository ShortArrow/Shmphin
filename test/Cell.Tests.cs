using main.model;

namespace test;

public class CellTests
{
  [Theory]
  [InlineData(1, 4, new byte[] { 1 }, new byte[] { 4 })]
  [InlineData(513, 1027, new byte[] { 1, 2 }, new byte[] { 3, 4 })]
  [InlineData(1, 4, new byte[] { 1, 0, 0, 0 }, new byte[] { 4, 0, 0, 0 })]
  public void TestCell(ulong BeforeValue, ulong currentValue, byte[] before, byte[] current)
  {
    Cell cell = new(before, current, 0, 0);
    Assert.Equal(BeforeValue, cell.BeforeValue);
    Assert.Equal(currentValue, cell.CurrentValue);
  }
}
