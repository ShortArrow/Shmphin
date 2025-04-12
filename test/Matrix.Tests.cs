using main.model;
using test.mock;

namespace test;

public class MatrixTests
{
  readonly MockConfig config = new(
      sharedMemoryName: "test",
      cellLength: 1,
      columnsLength: 8,
      sharedMemorySize: 16,
      sharedMemoryOffset: 0
    );

  readonly Matrix matrix ;
  public MatrixTests()
  {
    matrix = new(config, new main.memory.SnapShot(config));
    byte[] before = [
      0, 1, 2, 3, 4, 5, 6, 7,
      8, 9, 10, 11, 12, 13, 14, 15
    ];
    byte[] current = [
      100, 101, 102, 103, 104, 105, 106, 107,
      108, 109, 110, 111, 112, 113, 114, 115
    ];
    matrix.Update(before, current);
  }
  [Theory]
  [InlineData(new uint[] { 0, 0 }, new byte[] { 0 }, new byte[] { 100 })]
  [InlineData(new uint[] { 1, 0 }, new byte[] { 1 }, new byte[] { 101 })]
  [InlineData(new uint[] { 7, 0 }, new byte[] { 7 }, new byte[] { 107 })]
  [InlineData(new uint[] { 0, 1 }, new byte[] { 8 }, new byte[] { 108 })]
  [InlineData(new uint[] { 1, 1 }, new byte[] { 9 }, new byte[] { 109 })]
  [InlineData(new uint[] { 7, 1 }, new byte[] { 15 }, new byte[] { 115 })]
  public void TestUpdate(uint[] position, byte[] before, byte[] current)
  {
    Assert.Equal(current, matrix.GetCell(position[0], position[1]).Current);
    Assert.Equal(before, matrix.GetCell(position[0], position[1]).Before);
  }
  [Fact]
  public void TestGetColumn()
  {
    Assert.Equal(2, matrix.GetColumn(0).Length);
    Assert.Equal(2, matrix.GetColumn(7).Length);
    Assert.Equal(101, matrix.GetColumn(1)[0].Current[0]);
    Assert.Equal(109, matrix.GetColumn(1)[1].Current[0]);
  }
}
