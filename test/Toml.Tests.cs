using main.config;

namespace test;

public class TomlTests
{
  readonly CurrentConfig config = new(new(null, null, null));
  public TomlTests()
  {
    config.UpdateConfig(
      Path.GetFullPath(
        "../../../testdata/config.toml"
      )
    );
    config.Sync();
  }
  [Fact]
  public void ReadSharedMemoryName()
  {
    var actual = config.SharedMemoryName;
    var expected = "TESTshmTEST";
    Assert.Equal(expected, actual);
  }
  [Fact]
  public void ReadCellLength()
  {
    var actual = config.CellLength;
    var expected = (uint)1;
    Assert.Equal(expected, actual);
  }
  [Fact]
  public void ReadColumnsLength()
  {
    var actual = config.ColumnsLength;
    var expected = (uint)10;
    Assert.Equal(expected, actual);
  }
}
