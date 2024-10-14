using main.ui;
namespace test;

public class ParseTests
{
  [Theory]
  [InlineData("1", new byte[] { 1 })]
  [InlineData("2", new byte[] { 2 })]
  [InlineData("256", new byte[] { 2 })]
  public void TestParseNewValue(string input, byte[] expected)
  {
    var actual = Input.ParseNewValue(input);
    Assert.Equal(expected, actual);
  }

}
