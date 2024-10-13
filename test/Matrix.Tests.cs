using main.model;
using Spectre.Console;

namespace test;

public class MatrixTests
{
  Matrix matrix = new();
  public MatrixTests()
  {
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
}
