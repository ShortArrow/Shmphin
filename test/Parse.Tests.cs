using main.ui;
namespace test;

public class ParseTests
{
  [Theory]
  [InlineData("1", new byte[] { 1 })]
  [InlineData("01", new byte[] { 1 })]
  [InlineData("0001", new byte[] { 0, 1 })]
  [InlineData("0101", new byte[] { 1, 1 })]
  [InlineData("10101", new byte[] { 1, 1, 1 })]
  [InlineData("1010101", new byte[] { 1, 1, 1, 1 })]
  [InlineData("100FF01", new byte[] { 1, 0, 0xFF, 1 })]
  [InlineData("FF", new byte[] { 0xFF })]
  public void TestParseNewValue(string input, byte[] expected)
  {
    var actual = Input.ParseNewValue(input);
    Assert.Equal(expected, actual);
  }
}
